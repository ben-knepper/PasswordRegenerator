using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using PasswordGeneration;

using PasswordRegenerator.Models;
using PasswordRegenerator.Views;

namespace PasswordRegenerator.ViewModels
{
    public class GeneratorViewModel : BaseViewModel
    {
        public string MasterPassword { get; set; }
        public string Keyword { get; set; }
        public string OptionalKeyword { get; set; }
        public string Modifier { get; set; }
        public ParameterSet ParameterSet { get; set; }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public Command GenerateCommand { get; private set; }
        public Command NavigateToOptionsCommand { get; private set; }

        public GeneratorViewModel()
        {
            Title = "Generate";
            GenerateCommand = new Command(async () => await Task.Run(ExecuteGenerateCommand));

            var symbols = new UnitSet(UnitSet.AllKeyboardSymbols, Bounds.AtLeastOne);

            var unitSets = new List<UnitSet>()
            {
                new UnitSet(UnitSet.LowercaseLetters,
                    Bounds.AtLeastOne),
                new UnitSet(UnitSet.UppercaseLetters,
                    Bounds.AtLeastOne),
                new UnitSet(UnitSet.Numbers,
                    Bounds.AtLeastOne),
                symbols
            };

            ParameterSet = new ParameterSet()
            {
                Length = 10,
                UnitSets = new ObservableCollection<UnitSet>(unitSets),
                IsLegacy = true,
            };
        }

        void ExecuteGenerateCommand()
        {
            IsBusy = true;

            Password = PasswordGeneratorLegacy.Generate(
                MasterPassword, Keyword, OptionalKeyword, Modifier,
                ParameterSet.Length, ParameterSet.UnitSets);

            IsBusy = false;
        }
    }
}