using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PasswordGenerationLegacy
{
    public static class PasswordGenerator
    {
        static readonly SHA1 sha1 = SHA1.Create();

        /// <summary>
        /// Generates a password with the given key, keyword, characters, size, and optional keyword.
        /// </summary>
        /// <param name="key">The secret primary key.</param>
        /// <param name="keyword">The keyword used to generate a unique password</param>
        /// <param name="unitSet">The unit set available for generating the password with</param>
        /// <param name="size">The size of the password. Default = 8.</param>
        /// <param name="optionalKeyword">The optional keyword used to generate a unique password that uses the same keyword.</param>
        /// <returns>A string representing the password.</returns>
        public static string Generate(string key, string keyword, PasswordUnitListSet unitListSet,
            string subKeyword = null, string optionalKeyword = null, int size = 10)
        {
            #region Argument Validation

            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException(nameof(key));
            }
            if (String.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException(nameof(keyword));
            }
            if (unitListSet == null)
            {
                throw new ArgumentNullException(nameof(unitListSet));
            }

            #endregion

            // encrypt primary key
            byte[] primaryKey = GenerateKey(key);

            // encrypt secondary key and combine with primary
            byte[] secondaryKey = GenerateKey(keyword);
            primaryKey = CombineKeys(primaryKey, secondaryKey);

            // append the size to the optional key if there is one,
            // or make the size the optional key if there is not
            optionalKeyword = optionalKeyword != null ? optionalKeyword + size : size.ToString();

            // if there is a sub keyword, encrypt it and combine it with the primary key
            if (!String.IsNullOrEmpty(subKeyword))
            {
                byte[] subKey = GenerateKey(subKeyword);
                primaryKey = CombineKeys(primaryKey, subKey);
            }

            // encrypt the optional key and combine it with the primary
            byte[] optionalKey = GenerateKey(optionalKeyword);
            primaryKey = CombineKeys(primaryKey, optionalKey);
            
            // encrypt unitListSet and combine with primary key
            byte[] unitListSetKey = GenerateKey(unitListSet.ToString());
            primaryKey = CombineKeys(primaryKey, unitListSetKey);

            // generate the password format
            List<string>[] format = GenerateFormat(primaryKey, unitListSet, size);

            // generate and return the password
            return GeneratePassword(primaryKey, format);
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
        private static List<string>[] GenerateFormat(byte[] key, PasswordUnitListSet unitListSet, int size)
        {
            #region Argument Validation
            
            // throw exception if minumum characters greater than size
            long minSum = 0;
            foreach (int min in unitListSet.Select(uls => uls.Bounds.Min))
            {
                minSum += min;
            }
            if (minSum > size)
            {
                throw new ArgumentException("Sum of unitList minimums greater than password size");
            }

            // throw exception if maximum characters less than size
            long maxSum = 0;
            foreach (long max in unitListSet.Select(uls => uls.Bounds.Max))
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
            foreach (PasswordUnitList unitList in unitListSet)
            {
                for (int i = 0; i < unitList.Bounds.Min; ++i)
                {
                    format[order.Dequeue()] = unitList;
                }
            }

            // fill the rest of the format with lists of all remaining units
            List<string> combinedUnitList = new List<string>();
            List<Tuple<int, int, int>> unitListAmountsLeft = new List<Tuple<int, int, int>>(); // stores the amount left for each list
                // first in the tuple is the amount, second is the start range of the list in the combined list, third is the size of the list
            int currentIndex = 0; // keeps track of the index for unitListAmountsLeft
            foreach (PasswordUnitList unitList in unitListSet.Where(ul => ul.Bounds.Max - ul.Bounds.Min > 0))
            {
                combinedUnitList.AddRange(unitList);
                unitListAmountsLeft.Add(
                    new Tuple<int, int, int>(unitList.Bounds.Max - unitList.Bounds.Min, currentIndex, unitList.Count));
                currentIndex += unitList.Count;
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
