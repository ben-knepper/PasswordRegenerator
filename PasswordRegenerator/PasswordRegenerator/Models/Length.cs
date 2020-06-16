using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordRegenerator.Models
{
    public struct Length
    {
        public int Value { get; private set; }
        public string Name => Value.ToString();

        public Length(int value)
        {
            Value = value;
        }
    }
}
