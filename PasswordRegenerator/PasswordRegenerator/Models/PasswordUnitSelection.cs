using System;
using System.Collections.Generic;
using System.Text;

using PasswordGeneration;

namespace PasswordRegenerator.Models
{
    public class PasswordUnitSelection
    {
        public PasswordUnitSet UnitSet { get; set; }
        public Bounds Bounds { get; set; }
        public string ID => UnitSet.ID;
        public string Name => UnitSet.Name;
        public bool IsUserMade => UnitSet.IsUserMade;

        public PasswordUnitSelection(PasswordUnitSet unitSet, Bounds bounds)
        {
            UnitSet = unitSet;
            Bounds = bounds;
        }
    }
}
