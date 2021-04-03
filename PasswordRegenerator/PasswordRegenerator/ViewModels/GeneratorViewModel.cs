using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using PasswordGeneration;
using PasswordGeneration.Legacy;

using PasswordRegenerator.Models;
using PasswordRegenerator.Views;
using System.Linq;

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

            var unitSets = new List<PasswordUnitSelection>()
            {
                new PasswordUnitSelection(PasswordUnitSet.Lowercase,       Bounds.AtLeastOne),
                new PasswordUnitSelection(PasswordUnitSet.Uppercase,       Bounds.AtLeastOne),
                new PasswordUnitSelection(PasswordUnitSet.Numbers,         Bounds.AtLeastOne),
                new PasswordUnitSelection(PasswordUnitSet.CompleteSymbols, Bounds.AtLeastOne),
            };

            ParameterSet = new ParameterSet(10, unitSets, false);
        }

        void ExecuteGenerateCommand()
        {
            IsBusy = true;

            var unitSelections = ParameterSet.UnitSelections
                .Select(pus => new UnitSelection(pus.UnitSet, pus.Bounds))
                .ToList();

            if (ParameterSet.IsLegacy)
            {
                Password = PasswordGeneratorLegacy.Generate(
                    MasterPassword, Keyword, OptionalKeyword, Modifier,
                    ParameterSet.Length, unitSelections);
            }
            else
            {
                var generator = new PasswordGenerator()
                {
                    MasterPassword = MasterPassword,
                    Keyword = Keyword + "\n" + OptionalKeyword,
                    Modifier = Modifier,
                    Length = ParameterSet.Length,
                    UnitSets = unitSelections
                };
                Password = generator.Generate();
            }

            IsBusy = false;

        }
    }
}