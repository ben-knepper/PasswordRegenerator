using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordRegenerator.ViewModels
{
    public class LengthOption : NamedOption<int>
    {
        public LengthOption(int value) : base(value) { }

        protected override string GetNameFromValue() => Value.ToString();
    }
}
