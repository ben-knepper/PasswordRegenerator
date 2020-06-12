using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerationLegacy
{
    /// <summary>
    /// The set of PasswordUnitLists that are available when generating a password.
    /// </summary>
    public class PasswordUnitListSet : List<PasswordUnitList>
    {
        #region Constants
        /// <summary>
        /// At least one lowercase letter and one each of uppercase letters, numbers, and common symbols.
        /// </summary>
        public static readonly PasswordUnitListSet Default = new PasswordUnitListSet(
            new List<PasswordUnitList>
            {
                new PasswordUnitList(CommonPasswordUnitLists.LowercaseLetters, PasswordUnitListBounds.AtLeastOne),
                new PasswordUnitList(CommonPasswordUnitLists.UppercaseLetters, PasswordUnitListBounds.One),
                new PasswordUnitList(CommonPasswordUnitLists.Numbers, PasswordUnitListBounds.One),
                new PasswordUnitList(CommonPasswordUnitLists.CommonSymbols, PasswordUnitListBounds.One)
            });
        /// <summary>
        /// At least one lowercase letter, uppercase letter, number, and common symbol.
        /// </summary>
        public static readonly PasswordUnitListSet AtLeastOneOfEach = new PasswordUnitListSet(
            new List<PasswordUnitList>
            {
                new PasswordUnitList(CommonPasswordUnitLists.LowercaseLetters, PasswordUnitListBounds.AtLeastOne),
                new PasswordUnitList(CommonPasswordUnitLists.UppercaseLetters, PasswordUnitListBounds.AtLeastOne),
                new PasswordUnitList(CommonPasswordUnitLists.Numbers, PasswordUnitListBounds.AtLeastOne),
                new PasswordUnitList(CommonPasswordUnitLists.CommonSymbols, PasswordUnitListBounds.AtLeastOne)
            });
        /// <summary>
        /// Any number of lowercase letters, uppercase letters, numbers, and all symbols.
        /// </summary>
        public static readonly PasswordUnitListSet NoBounds = new PasswordUnitListSet(
            new List<PasswordUnitList>
            {
                new PasswordUnitList(CommonPasswordUnitLists.AllAlphanumericsAndKeyboardSymbols, PasswordUnitListBounds.Any)
            });
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an empty instance of a PasswordUnitListSet
        /// </summary>
        public PasswordUnitListSet() : base() { }
        /// <summary>
        /// Creates a new instance of a PasswordUnitListSet using elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The collection to copy.</param>
        public PasswordUnitListSet(IEnumerable<PasswordUnitList> collection) : base(collection) { }
        /// <summary>
        /// Creaties an empty instance of a PasswordUnitListSet with the given capacity.
        /// </summary>
        /// <param name="capacity">The starting capacity.</param>
        public PasswordUnitListSet(int capacity) : base(capacity) { }
        #endregion

        public override string ToString()
        {
            // creates a string listing the PasswordUnitLists and bounds, e.g.:
            //     {(a,b,c,d,...),(1,2147483647)}
            //     {(1,2,3,4,...),(1, 1)}
            StringBuilder result = new StringBuilder();
            foreach (PasswordUnitList ul in this)
                result.Append("{" + ul.ToString() + "," + ul.Bounds.ToString() + "}\n");
            result.Remove(result.Length - 1, 1); // remove the last newline
            return result.ToString();
        }
    }
}