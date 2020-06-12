using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PasswordGeneration
{
    public class UnitSet : ReadOnlyCollection<string>
    {
        public static readonly ReadOnlyCollection<string> AllAlphanumericsAndKeyboardSymbols =
            new ReadOnlyCollection<string>(Enumerable.Range(32, 95).Select(i => String.Empty + (char)i).ToList());
        public static readonly ReadOnlyCollection<string> LowercaseLetters =
            new ReadOnlyCollection<string>(Enumerable.Range('a', 26).Select(i => String.Empty + (char)i).ToList());
        public static readonly ReadOnlyCollection<string> UppercaseLetters =
            new ReadOnlyCollection<string>(Enumerable.Range('A', 26).Select(i => String.Empty + (char)i).ToList());
        public static readonly ReadOnlyCollection<string> Numbers =
            new ReadOnlyCollection<string>(Enumerable.Range('0', 10).Select(i => String.Empty + (char)i).ToList());
        public static readonly ReadOnlyCollection<string> CommonSymbols =
            new ReadOnlyCollection<string>(new List<string> { "!", "@", "#", "$", "%", "^", "&", "*", "?" });
        public static readonly ReadOnlyCollection<string> AllKeyboardSymbolsAndSpace =
            new ReadOnlyCollection<string>(AllAlphanumericsAndKeyboardSymbols.Where(
                s => !LowercaseLetters.Contains(s) && !UppercaseLetters.Contains(s) && !Numbers.Contains(s)).ToList());
        public static readonly ReadOnlyCollection<string> AllKeyboardSymbols =
            new ReadOnlyCollection<string>(AllKeyboardSymbolsAndSpace.Where(s => s != " ").ToList());

        public Bounds Bounds { get; set; }

        public UnitSet(IList<string> list, Bounds bounds) : base(list)
        {
            Bounds = bounds;
        }

        public static implicit operator List<string>(UnitSet unitSet)
        {
            return unitSet.ToList();
        }
        
        public override string ToString()
        {
            // Result looks like "{a,b,c,...,z}[1,2147483647]"
            StringBuilder result = new StringBuilder("{");
            foreach (string s in this)
                result.Append(s + ",");
            result.Remove(result.Length - 1, 1); // remove the last comma
            result.Append($"}}{Bounds}");
            return result.ToString();
        }
    }
}
