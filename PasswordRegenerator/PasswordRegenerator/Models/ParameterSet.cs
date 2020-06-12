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
        public ObservableCollection<UnitSet> UnitSets { get; set; }
        public bool IsLegacy { get; set; }

        public ParameterSet() { }
        public ParameterSet(int length, ICollection<UnitSet> unitSets, bool isLegacy)
        {
            Length = length;
            UnitSets = new ObservableCollection<UnitSet>(unitSets);
            IsLegacy = isLegacy;
        }

        public ParameterSet Copy()
        {
            var unitSetsCopy = new UnitSet[UnitSets.Count];
            UnitSets.CopyTo(unitSetsCopy, 0);
            return new ParameterSet(Length, unitSetsCopy, IsLegacy);
        }
    }
}
