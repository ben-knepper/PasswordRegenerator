using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using PasswordGeneration;
using PasswordRegenerator.Models;
using Xamarin.Forms;

namespace PasswordRegenerator.ViewModels
{
    class GeneratorOptionsViewModel : BaseViewModel
    {
        private static readonly ReadOnlyCollection<LengthOption> _lengthOptions =
            new ReadOnlyCollection<LengthOption>(
                Enumerable.Range(1, 256).Select(x => new LengthOption(x)).ToList()
            );
        private static readonly ReadOnlyCollection<NamedOption<Bounds>> _boundsOptions =
            new ReadOnlyCollection<NamedOption<Bounds>>(new List<NamedOption<Bounds>>()
                {
                    new NamedOption<Bounds>(Bounds.None,       "None"),
                    new NamedOption<Bounds>(Bounds.One,        "One"),
                    new NamedOption<Bounds>(Bounds.AtLeastOne, "At least one"),
                    new NamedOption<Bounds>(Bounds.Any,        "Any number"),
                });
        private static readonly ReadOnlyCollection<NamedOption<IList<string>>> _symbolSetOptions =
            new ReadOnlyCollection<NamedOption<IList<string>>>(new List<NamedOption<IList<string>>>()
            {
                new NamedOption<IList<string>>(PasswordUnitSet.SimpleSymbols, "Common"),
                new NamedOption<IList<string>>(PasswordUnitSet.CompleteSymbols, "Complete"),
                new NamedOption<IList<string>>(null, "Custom"),
            });

        private PasswordUnitSelection LowercaseSelection => CurrentParameterSet["lowercase"];
        private PasswordUnitSelection UppercaseSelection => CurrentParameterSet["uppercase"];
        private PasswordUnitSelection NumberSelection    => CurrentParameterSet["numbers"];
        private PasswordUnitSelection SymbolSelection    => CurrentParameterSet.FirstStartingWith("symbols");

        public ParameterSet OriginalParameterSet { get; set; }
        public ParameterSet CurrentParameterSet { get; set; }

        public ReadOnlyCollection<LengthOption>               LengthOptions          => _lengthOptions;
        public ReadOnlyCollection<NamedOption<Bounds>>        LowercaseBoundsOptions => _boundsOptions;
        public ReadOnlyCollection<NamedOption<Bounds>>        UppercaseBoundsOptions => _boundsOptions;
        public ReadOnlyCollection<NamedOption<Bounds>>        NumberBoundsOptions    => _boundsOptions;
        public ReadOnlyCollection<NamedOption<Bounds>>        SymbolBoundsOptions    => _boundsOptions;
        public ReadOnlyCollection<NamedOption<IList<string>>> SymbolSetOptions       => _symbolSetOptions;

        private LengthOption _selectedLength;
        public LengthOption SelectedLength
        {
            get { return _selectedLength; }
            set
            {
                _selectedLength = value;
                CurrentParameterSet.Length = _selectedLength.Value;
            }
        }
        private NamedOption<Bounds> _selectedLowercaseBounds;
        public NamedOption<Bounds> SelectedLowercaseBounds
        {
            get { return _selectedLowercaseBounds; }
            set
            {
                _selectedLowercaseBounds = value;
                LowercaseSelection.Bounds = _selectedLowercaseBounds.Value;
            }
        }
        private NamedOption<Bounds> _selectedUppercaseBounds;
        public NamedOption<Bounds> SelectedUppercaseBounds
        {
            get { return _selectedUppercaseBounds; }
            set
            {
                _selectedUppercaseBounds = value;
                UppercaseSelection.Bounds = _selectedUppercaseBounds.Value;
            }
        }
        private NamedOption<Bounds> _selectedNumberBounds;
        public NamedOption<Bounds> SelectedNumberBounds
        {
            get { return _selectedNumberBounds; }
            set
            {
                _selectedNumberBounds = value;
                NumberSelection.Bounds = _selectedNumberBounds.Value;
            }
        }
        private NamedOption<Bounds> _selectedSymbolBounds;
        public NamedOption<Bounds> SelectedSymbolBounds
        {
            get { return _selectedSymbolBounds; }
            set
            {
                _selectedSymbolBounds = value;
                SymbolSelection.Bounds = _selectedSymbolBounds.Value;
            }
        }
        private NamedOption<IList<string>> _selectedSymbolSet;
        public NamedOption<IList<string>> SelectedSymbolSet
        {
            get { return _selectedSymbolSet; }
            set
            {
                _selectedSymbolSet = value;
                SymbolSelection.UnitSet = _selectedSymbolSet?.Value as PasswordUnitSet ?? CustomSymbolSet;
            }
        }
        private string _customSymbolSetString;
        public string CustomSymbolSetString
        {
            get { return _customSymbolSetString; }
            set
            {
                _customSymbolSetString = value;
                if (SelectedSymbolSet.Value is null)
                    SymbolSelection.UnitSet = CustomSymbolSet;
            }
        }

        private PasswordUnitSet CustomSymbolSet
            => new PasswordUnitSet("symbols,custom", "Custom Symbols",
                ConvertStringToList(CustomSymbolSetString));

        public Command SaveCommand { get; private set; }
        public Command CancelCommand { get; private set; }

        public GeneratorOptionsViewModel(GeneratorViewModel parentViewModel)
        {
            Title = "Generation Options";

            CurrentParameterSet  = parentViewModel.ParameterSet;
            OriginalParameterSet = CurrentParameterSet.Copy();

            SaveCommand   = new Command(ExecuteSaveCommand);
            CancelCommand = new Command(ExecuteCancelCommand);

            SetSelectedItems();
        }

        void ExecuteSaveCommand()
        {
            // do nothing
        }
        void ExecuteCancelCommand()
        {
            ResetParameters();
        }

        private void SetSelectedItems()
        {
            SelectedLength = _lengthOptions.First(
                option => option.Value == CurrentParameterSet.Length);
            SelectedLowercaseBounds = _boundsOptions.First(
                option => option.Value == LowercaseSelection.Bounds);
            SelectedUppercaseBounds = _boundsOptions.First(
                option => option.Value == UppercaseSelection.Bounds);
            SelectedNumberBounds = _boundsOptions.First(
                option => option.Value == NumberSelection.Bounds);
            SelectedSymbolBounds = _boundsOptions.First(
                option => option.Value == SymbolSelection.Bounds);
            SelectedSymbolSet = _symbolSetOptions.FirstOrDefault(
                option => option.Value == SymbolSelection.UnitSet);
            if (SelectedSymbolSet is null)
            {
                SelectedSymbolSet = _symbolSetOptions.Last(o => o.Name == "Custom");
                CustomSymbolSetString = ConvertListToString(SymbolSelection.UnitSet);
            }
        }
        private void ResetParameters()
        {
            CurrentParameterSet.Length   = OriginalParameterSet.Length;
            LowercaseSelection.Bounds    = OriginalParameterSet["lowercase"].Bounds;
            UppercaseSelection.Bounds    = OriginalParameterSet["uppercase"].Bounds;
            NumberSelection.Bounds       = OriginalParameterSet["numbers"].Bounds;
            SymbolSelection.Bounds       = OriginalParameterSet.FirstStartingWith("symbols").Bounds;
            SymbolSelection.UnitSet      = OriginalParameterSet.FirstStartingWith("symbols").UnitSet;
            CurrentParameterSet.IsLegacy = OriginalParameterSet.IsLegacy;
        }

        private static List<string> ConvertStringToList(string s)
            => s?.Select(c => c.ToString())?.ToList();
        private static string ConvertListToString(IList<string> list)
            => list.Aggregate((current, next) => current + next);
    }
}
