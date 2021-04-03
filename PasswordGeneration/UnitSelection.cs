using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PasswordGeneration
{
    public class UnitSelection : ReadOnlyCollection<string>
    {
        public Bounds Bounds { get; set; }

        public UnitSelection(IList<string> list, Bounds bounds)
            : base(list)
        {
            Bounds = bounds;
        }

        public static implicit operator List<string>(UnitSelection unitSet) => unitSet.ToList();
        
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
