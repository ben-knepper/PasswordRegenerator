using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerationLegacy
{
    /// <summary>
    /// Gives quick access to common password unit sets.
    /// </summary>
    public static class CommonPasswordUnitLists
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
    }
}
