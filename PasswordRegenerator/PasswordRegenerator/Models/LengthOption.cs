using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordRegenerator.Models
{
    public struct LengthOption
    {
        public int Value { get; private set; }
        public string Name => Value.ToString();

        public LengthOption(int value)
        {
            Value = value;
        }
    }
}
