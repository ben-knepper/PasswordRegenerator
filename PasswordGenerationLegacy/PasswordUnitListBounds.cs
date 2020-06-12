using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerationLegacy
{
    /// <summary>
    /// Represents the minimum and maximum amount of units from a PasswordUnitList.
    /// </summary>
    public struct PasswordUnitListBounds
    {
        #region Constants
        /// <summary>
        /// Zero to all characters (0, Int32.MaxValue).
        /// </summary>
        public static readonly PasswordUnitListBounds Any = new PasswordUnitListBounds(0, Int32.MaxValue);
        /// <summary>
        /// Zero characters (0, 0).
        /// </summary>
        public static readonly PasswordUnitListBounds None = new PasswordUnitListBounds(0, 0);
        /// <summary>
        /// One character (1, 1).
        /// </summary>
        public static readonly PasswordUnitListBounds One = new PasswordUnitListBounds(1, 1);
        /// <summary>
        /// At least one character (1, Int32.MaxValue).
        /// </summary>
        public static readonly PasswordUnitListBounds AtLeastOne = new PasswordUnitListBounds(1, Int32.MaxValue);
        #endregion

        #region Properties
        /// <summary>
        /// The minimum amount of units from the list.
        /// </summary>
        public int Min { get; private set; }
        /// <summary>
        /// The maximum amount of units from the list. Limitless = Int32.MaxValue.
        /// </summary>
        public int Max { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PasswordUnitListBounds class.
        /// </summary>
        /// <param name="min">The minimum amount of units from the list.</param>
        /// <param name="max">The maximum amount of units from the list. Limitless = Int32.MaxValue.</param>
        public PasswordUnitListBounds(int min, int max)
        {
            Min = min;
            Max = max;
        }
        #endregion

        public override string ToString() => $"({Min},{Max})";
        public static bool operator ==(PasswordUnitListBounds bounds1, PasswordUnitListBounds bounds2)
        {
            return bounds1.Min == bounds2.Min && bounds1.Max == bounds2.Max;
        }
        public static bool operator !=(PasswordUnitListBounds bounds1, PasswordUnitListBounds bounds2)
        {
            return bounds1.Min != bounds2.Min || bounds1.Max != bounds2.Max;
        }
        public override bool Equals(object obj)
        {
            if (obj is PasswordUnitListBounds) return this == (PasswordUnitListBounds)obj;
            else return false;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
