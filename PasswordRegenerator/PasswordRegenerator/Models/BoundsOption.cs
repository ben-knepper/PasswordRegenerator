using System;
using System.Collections.Generic;
using System.Text;

using PasswordGeneration;

namespace PasswordRegenerator.Models
{
    public struct BoundsOption
    {
        public Bounds Value { get; private set; }
        public string Name { get; private set; }

        public BoundsOption(Bounds value, string name)
        {
            Value = value;
            Name = name;
        }
        public BoundsOption(Bounds value) : this(value, value.ToString()) { }
    }
}
