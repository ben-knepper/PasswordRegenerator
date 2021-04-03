using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PasswordRegenerator.Models
{
    public class ParameterSet
    {
        public int Length { get; set; }
        public ObservableCollection<PasswordUnitSelection> UnitSelections { get; set; }
        public bool IsLegacy { get; set; }

        public PasswordUnitSelection this[string id] => UnitSelections.FirstOrDefault(us => us.ID.Equals(id));

        public ParameterSet() { }
        public ParameterSet(int length, IList<PasswordUnitSelection> unitSets)
            : this(length, unitSets, false) { }
        public ParameterSet(int length, IList<PasswordUnitSelection> unitSets, bool isLegacy)
        {
            Length = length;
            UnitSelections = new ObservableCollection<PasswordUnitSelection>(unitSets);
            IsLegacy = isLegacy;
        }

        public PasswordUnitSelection FirstStartingWith(string substring) => UnitSelections.First(us => us.ID.StartsWith(substring));

        /// <summary>
        /// Deep copies the parameter set.
        /// </summary>
        /// <returns>A deep copy of the parameter set.</returns>
        public ParameterSet Copy()
        {
            var newUnitSets = new ObservableCollection<PasswordUnitSelection>(
                UnitSelections.Select(us => new PasswordUnitSelection(us.UnitSet, us.Bounds)));
            return new ParameterSet(Length, newUnitSets, IsLegacy);
        }
    }
}
