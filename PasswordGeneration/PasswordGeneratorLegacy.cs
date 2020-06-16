using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PasswordGeneration
{
    public static class PasswordGeneratorLegacy
    {
        static readonly SHA1 sha1 = SHA1.Create();

        public static string Generate(string master, string keyword, string optionalKeyword,
            string modifier, int size, IList<UnitSet> unitSets)
        {
            #region Argument Validation

            if (String.IsNullOrEmpty(master))
            {
                throw new ArgumentException(nameof(master));
            }
            if (String.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException(nameof(keyword));
            }
            if (unitSets == null)
            {
                throw new ArgumentNullException(nameof(unitSets));
            }

            #endregion

            // encrypt master password
            byte[] primaryKey = GenerateKey(master);

            // encrypt keyword and combine with master
            byte[] secondaryKey = GenerateKey(keyword);
            primaryKey = CombineKeys(primaryKey, secondaryKey);

            // append the size to the modifier if there is one,
            // or make the size the modifier if there is not
            modifier = modifier != null ? modifier + size : size.ToString();

            // if there is an optional keyword, encrypt it and combine it with the primary key
            if (!String.IsNullOrEmpty(optionalKeyword))
            {
                byte[] subKey = GenerateKey(optionalKeyword);
                primaryKey = CombineKeys(primaryKey, subKey);
            }

            // encrypt the modifier key and combine it with the primary
            byte[] modifierKey = GenerateKey(modifier);
            primaryKey = CombineKeys(primaryKey, modifierKey);
            
            // encrypt unitListSet and combine with primary key
            byte[] unitListSetKey = GenerateKey(ConvertUnitSetsToString(unitSets));
            primaryKey = CombineKeys(primaryKey, unitListSetKey);

            // generate the password format
            List<string>[] format = GenerateFormat(primaryKey, unitSets, size);

            // generate and return the password
            return GeneratePassword(primaryKey, format);
        }
        private static string ConvertUnitSetsToString(IList<UnitSet> unitSets)
        {
            var builder = new StringBuilder();
            foreach (var unitSet in unitSets)
            {
                // make compatible with legacy format
                string setString = unitSet.ToString();
                var setBuilder = new StringBuilder(unitSet.ToString());
                setBuilder.Replace("}[", "),(", setString.LastIndexOf("}["), 2);
                setBuilder.Replace("{", "{(", 0, 2);
                setBuilder.Replace("]", ")}", setBuilder.Length - 1, 1);

                builder.Append($"{setBuilder}\n");
            }
            builder.Remove(builder.Length - 1, 1); // remove the last newline

            return builder.ToString();
        }
        private static byte[] GenerateKey(string keyString)
        {
            #region Argument Validation

            if (String.IsNullOrEmpty(keyString))
            {
                throw new ArgumentException("Key string cannot be null or empty", nameof(keyString));
            }

            #endregion

            byte[] bytes = Encoding.Unicode.GetBytes(keyString);
            return sha1.ComputeHash(bytes);
        }
        private static byte[] CombineKeys(byte[] key1, byte[] key2)
        {
            // append the second key to the first
            byte[] bytes = new byte[key1.Length + key2.Length];
            Array.Copy(key1, bytes, key1.Length);
            Array.Copy(key2, 0, bytes, key1.Length, key2.Length);

            // create a new hash using the combined keys
            return sha1.ComputeHash(bytes);
        }
        private static ulong ExtractFromAndScrambleKey(byte[] key)
        {
            // construct a long from the first four bytes of the key
            ulong output = 0;
            for (int i = 0; i < 8; ++i)
            {
                output <<= 8; // shift left one byte to make room for the next
                output |= key[i]; // OR the return value with the next byte
            }

            // create a new hash from the old one
            byte[] bytes = sha1.ComputeHash(key);
            Array.Copy(bytes, key, 20);

            // return
            return output;
        }
        private static List<string>[] GenerateFormat(byte[] key, IList<UnitSet> unitSets, int size)
        {
            #region Argument Validation
            
            // throw exception if minumum characters greater than size
            long minSum = 0;
            foreach (int min in unitSets.Select(uls => uls.Bounds.Min))
            {
                minSum += min;
            }
            if (minSum > size)
            {
                throw new ArgumentException("Sum of unitList minimums greater than password size");
            }

            // throw exception if maximum characters less than size
            long maxSum = 0;
            foreach (long max in unitSets.Select(uls => uls.Bounds.Max))
            {
                maxSum += max;
            }
            if (maxSum < size)
            {
                throw new ArgumentException("Sum of unitList maximums less than password size");
            }

            #endregion

            #region Generate Order

            Queue<int> order = new Queue<int>(size); // gives the order in which to fill the format
            List<int> indeces = Enumerable.Range(0, size).ToList(); // list of 1-[size] to take ordinals from
            while (indeces.Count > 1) // stop when only one value left, since last order has to be last in indeces
            {
                // get a "random" value from the key
                ulong x = ExtractFromAndScrambleKey(key);

                // get index with modulo then get value from indeces and remove
                int index = (int)(x % (byte)indeces.Count);
                order.Enqueue(indeces[index]);
                indeces.RemoveAt(index);
            }
            // fill last in order with last remaining value of indeces,
            // since random selection of one value is counterproductive
            order.Enqueue(indeces[0]);

            #endregion

            #region Generate Format

            // create return value
            List<string>[] format = new List<string>[size];

            // fill format with minimums
            foreach (var unitSet in unitSets)
            {
                for (int i = 0; i < unitSet.Bounds.Min; ++i)
                {
                    format[order.Dequeue()] = unitSet;
                }
            }

            // fill the rest of the format with lists of all remaining units
            List<string> combinedUnitList = new List<string>();
            List<Tuple<int, int, int>> unitListAmountsLeft = new List<Tuple<int, int, int>>(); // stores the amount left for each list
                // first in the tuple is the amount, second is the start range of the list in the combined list, third is the size of the list
            int currentIndex = 0; // keeps track of the index for unitListAmountsLeft
            foreach (var unitSet in unitSets.Where(ul => ul.Bounds.Max - ul.Bounds.Min > 0))
            {
                combinedUnitList.AddRange(unitSet);
                unitListAmountsLeft.Add(
                    new Tuple<int, int, int>(unitSet.Bounds.Max - unitSet.Bounds.Min, currentIndex, unitSet.Count));
                currentIndex += unitSet.Count;
            }
            while (order.Count > 0)
            {
                format[order.Dequeue()] = combinedUnitList;
                
                // decrement all unit list amounts and remove any unit lists that reach max
                for (int i = 0; i < unitListAmountsLeft.Count; ++i)
                {
                    // decrement amount
                    Tuple<int, int, int> tuple = unitListAmountsLeft[i];
                    unitListAmountsLeft[i] = new Tuple<int, int, int>(tuple.Item1 - 1, tuple.Item2, tuple.Item3);
                    
                    // if amount reaches zero, remove list
                    if (unitListAmountsLeft[i].Item1 <= 0)
                    {
                        List<string> newCombinedUnitList = new List<string>(combinedUnitList);
                        newCombinedUnitList.RemoveRange(tuple.Item2, tuple.Item3);
                        unitListAmountsLeft.RemoveAt(i);
                        combinedUnitList = newCombinedUnitList;

                        // adjust following amount tuples to compensate
                        for (int j = i; j < unitListAmountsLeft.Count; ++j)
                        {
                            unitListAmountsLeft[j] = new Tuple<int, int, int>(
                                unitListAmountsLeft[j].Item1,
                                unitListAmountsLeft[j].Item2 - tuple.Item3,
                                unitListAmountsLeft[j].Item3);
                        }
                    }
                }
            }

            // if a spaces are possible in the first and/or last spots, get rid of them
            if (format[0].Contains(" "))
            {
                List<string> newUnitList = new List<string>(format[0]);
                newUnitList.Remove(" ");
                format[0] = newUnitList;
            }
            if (format[size - 1].Contains(" "))
            {
                List<string> newUnitList = new List<string>(format[size - 1]);
                newUnitList.Remove(" ");
                format[size - 1] = newUnitList;
            }

            #endregion

            return format;
        }
        private static string GeneratePassword(byte[] key, List<string>[] format)
        {
            StringBuilder passwordBuilder = new StringBuilder();

            for (int i = 0; i < format.Length; ++i)
            {
                // get a "random" value from the key
                ulong x = ExtractFromAndScrambleKey(key);

                // get index with modulo then use to select a password unit
                int index = (int)(x % (byte)format[i].Count);
                passwordBuilder.Append(format[i][index]);
            }

            return passwordBuilder.ToString();
        }
    }
}
