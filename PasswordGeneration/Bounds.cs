using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGeneration
{
    public struct Bounds
    {
        /// <summary>
        /// Used to designate an unlimited number of units.
        /// </summary>
        public static readonly int Unlimited = Int32.MaxValue;
        /// <summary>
        /// Zero to all characters (0, Unlimited).
        /// </summary>
        public static readonly Bounds Any = new Bounds(0, Unlimited);
        /// <summary>
        /// Zero characters (0, 0).
        /// </summary>
        public static readonly Bounds None = new Bounds(0, 0);
        /// <summary>
        /// One character (1, 1).
        /// </summary>
        public static readonly Bounds One = new Bounds(1, 1);
        /// <summary>
        /// At least one character (1, Unlimited).
        /// </summary>
        public static readonly Bounds AtLeastOne = new Bounds(1, Unlimited);

        public int Min { get; set; }
        public int Max { get; set; }

        public Bounds (int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString() => $"[{Min},{Max}]";
        public static bool operator ==(Bounds bounds1, Bounds bounds2)
        {
            return bounds1.Min == bounds2.Min && bounds1.Max == bounds2.Max;
        }
        public static bool operator !=(Bounds bounds1, Bounds bounds2)
        {
            return bounds1.Min != bounds2.Min || bounds1.Max != bounds2.Max;
        }
        public override bool Equals(object obj)
        {
            if (obj is Bounds) return this == (Bounds)obj;
            else return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int rotatedMax = Max << 16 | Max >> 16;
                return (Min * 23) ^ rotatedMax;
            }
        }
    }
}
