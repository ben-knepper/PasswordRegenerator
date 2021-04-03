using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordRegenerator.ViewModels
{
    public class NamedOption<T>
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set { _value = value; Name = GetNameFromValue(); }
        }
        public string Name { get; protected set; }

        public NamedOption(T value)
        {
            Value = value;
        }
        public NamedOption(T value, string name)
        {
            Value = value;
            Name = name;
        }

        protected virtual string GetNameFromValue() => Name;
    }
}
