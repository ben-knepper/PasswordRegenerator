using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using PasswordGeneration;

namespace PasswordRegenerator.Models
{
    public class ParameterSet
    {
        public int Length { get; set; }
        public Bounds LowercaseBounds { get; set; }
        public Bounds UppercaseBounds { get; set; }
        public Bounds NumberBounds { get; set; }
        public Bounds SymbolBounds { get; set; }
        public bool IsLegacy { get; set; }

        public ParameterSet() { }
        public ParameterSet(int length, Bounds lowercaseBounds, Bounds uppercaseBounds,
            Bounds numberBounds, Bounds symbolBounds, bool isLegacy)
        {
            Length = length;
            LowercaseBounds = lowercaseBounds;
            UppercaseBounds = uppercaseBounds;
            NumberBounds = numberBounds;
            SymbolBounds = symbolBounds;
            IsLegacy = isLegacy;
        }

        public ParameterSet Copy()
        {
            return new ParameterSet(Length, LowercaseBounds, UppercaseBounds,
                NumberBounds, SymbolBounds, IsLegacy);
        }
    }
}
