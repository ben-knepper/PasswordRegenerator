using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PasswordRegenerator.Models
{
    public class PasswordUnitSet : ReadOnlyCollection<string>
    {
        public static readonly PasswordUnitSet Lowercase =
            new PasswordUnitSet("lowercase", "Lowercase Letters",
                Enumerable.Range('a', 26).Select(i => String.Empty + (char)i).ToList(),
                true);
        public static readonly PasswordUnitSet Uppercase =
            new PasswordUnitSet("uppercase", "Uppercase Letters",
                Enumerable.Range('A', 26).Select(i => String.Empty + (char)i).ToList(),
                true);
        public static readonly PasswordUnitSet Numbers =
            new PasswordUnitSet("numbers", "Numbers",
                Enumerable.Range('0', 10).Select(i => String.Empty + (char)i).ToList(),
                true);
        public static readonly PasswordUnitSet SimpleSymbols =
            new PasswordUnitSet("symbols,simple", "Simple Symbols",
                @"!@#$%^&*?".Select(c => c.ToString()).ToList(),
                true);
        public static readonly ReadOnlyCollection<string> AllAlphanumericsAndKeyboardSymbols =
            new ReadOnlyCollection<string>(Enumerable.Range(32, 95).Select(i => String.Empty + (char)i).ToList());
        // In ASCII order. Changing breaks regeneration consistency.
        public static readonly PasswordUnitSet CompleteSymbols =
            new PasswordUnitSet("symbols,complete", "Complete Symbols",
                @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~".Select(c => c.ToString()).ToList(),
                true);

        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsUserMade { get; set; }

        public PasswordUnitSet(string id, string name, IList<string> unitSet)
            : base(unitSet)
        {
            ID = id;
            Name = name;
        }
        public PasswordUnitSet(string id, string name, IList<string> unitSet, bool isUserMade)
            : this(id, name, unitSet)
        {
            IsUserMade = isUserMade;
        }
    }
}
