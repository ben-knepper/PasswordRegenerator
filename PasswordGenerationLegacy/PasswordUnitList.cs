using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerationLegacy
{
    /// <summary>
    /// Represents a list of units that are available when generating a password.
    /// </summary>
    public class PasswordUnitList : ReadOnlyCollection<string>
    {
        #region Properties
        /// <summary>
        /// The minimum and maximum amounts of units from the list to be used.
        /// </summary>
        public PasswordUnitListBounds Bounds { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of a PasswordUnitList
        /// </summary>
        /// <param name="list">The list of password units.</param>
        /// <param name="bounds">The minimum and maximum amounts of units from the list to be used.</param>
        public PasswordUnitList(IList<string> list, PasswordUnitListBounds bounds)
            : base(list)
        {
            Bounds = bounds;
        }
        #endregion

        public static implicit operator List<string>(PasswordUnitList unitList)
        {
            return unitList.ToList();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("(");
            foreach (string s in this)
                result.Append(s + ",");
            result.Remove(result.Length - 1, 1); // remove the last comma
            result.Append(")");
            return result.ToString();
        }
    }
}
