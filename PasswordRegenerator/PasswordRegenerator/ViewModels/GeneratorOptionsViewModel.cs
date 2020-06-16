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
        private static readonly ReadOnlyCollection<BoundsOption> _boundsOptions =
            new ReadOnlyCollection<BoundsOption>(new List<BoundsOption>()
            {
                new BoundsOption(Bounds.None, "None"),
                new BoundsOption(Bounds.One, "One"),
                new BoundsOption(Bounds.AtLeastOne, "At least one"),
                new BoundsOption(Bounds.Any, "Any number"),
            }
        );

        public ParameterSet OriginalParameterSet { get; set; }
        public ParameterSet CurrentParameterSet { get; set; }

        public ReadOnlyCollection<LengthOption> LengthOptions          => _lengthOptions;
        public ReadOnlyCollection<BoundsOption> LowercaseBoundsOptions => _boundsOptions;
        public ReadOnlyCollection<BoundsOption> UppercaseBoundsOptions => _boundsOptions;
        public ReadOnlyCollection<BoundsOption> NumberBoundsOptions    => _boundsOptions;
        public ReadOnlyCollection<BoundsOption> SymbolBoundsOptions    => _boundsOptions;

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
        private BoundsOption _selectedLowercaseBounds;
        public BoundsOption SelectedLowercaseBounds
        {
            get { return _selectedLowercaseBounds; }
            set
            {
                _selectedLowercaseBounds = value;
                CurrentParameterSet.LowercaseBounds = _selectedLowercaseBounds.Value;
            }
        }
        private BoundsOption _selectedUppercaseBounds;
        public BoundsOption SelectedUppercaseBounds
        {
            get { return _selectedUppercaseBounds; }
            set
            {
                _selectedUppercaseBounds = value;
                CurrentParameterSet.UppercaseBounds = _selectedUppercaseBounds.Value;
            }
        }
        private BoundsOption _selectedNumberBounds;
        public BoundsOption SelectedNumberBounds
        {
            get { return _selectedNumberBounds; }
            set
            {
                _selectedNumberBounds = value;
                CurrentParameterSet.NumberBounds = _selectedNumberBounds.Value;
            }
        }
        private BoundsOption _selectedSymbolBounds;
        public BoundsOption SelectedSymbolBounds
        {
            get { return _selectedSymbolBounds; }
            set
            {
                _selectedSymbolBounds = value;
                CurrentParameterSet.SymbolBounds = _selectedSymbolBounds.Value;
            }
        }

        public Command SaveCommand { get; private set; }
        public Command CancelCommand { get; private set; }

        public GeneratorOptionsViewModel(GeneratorViewModel parentViewModel)
        {
            Title = "Generation Options";

            CurrentParameterSet  = parentViewModel.ParameterSet;
            OriginalParameterSet = CurrentParameterSet.Copy();

            SaveCommand   = new Command(ExecuteSaveCommand);
            CancelCommand = new Command(ExecuteCancelCommand);

            SelectedLength = _lengthOptions.First(
                option => option.Value == OriginalParameterSet.Length);
            SelectedLowercaseBounds = _boundsOptions.First(
                option => option.Value == OriginalParameterSet.LowercaseBounds);
            SelectedUppercaseBounds = _boundsOptions.First(
                option => option.Value == OriginalParameterSet.UppercaseBounds);
            SelectedNumberBounds = _boundsOptions.First(
                option => option.Value == OriginalParameterSet.NumberBounds);
            SelectedSymbolBounds = _boundsOptions.First(
                option => option.Value == OriginalParameterSet.SymbolBounds);
        }

        void ExecuteSaveCommand()
        {
            // do nothing
        }
        void ExecuteCancelCommand()
        {
            CurrentParameterSet.Length          = OriginalParameterSet.Length;
            CurrentParameterSet.LowercaseBounds = OriginalParameterSet.LowercaseBounds;
            CurrentParameterSet.UppercaseBounds = OriginalParameterSet.UppercaseBounds;
            CurrentParameterSet.NumberBounds    = OriginalParameterSet.NumberBounds;
            CurrentParameterSet.SymbolBounds    = OriginalParameterSet.SymbolBounds;
            CurrentParameterSet.IsLegacy        = OriginalParameterSet.IsLegacy;
        }
    }
}
