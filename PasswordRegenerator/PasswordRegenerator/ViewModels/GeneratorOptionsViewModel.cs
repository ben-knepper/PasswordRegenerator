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
        private static readonly ReadOnlyCollection<Length> _lengthOptions =
            new ReadOnlyCollection<Length>(
                Enumerable.Range(1, 256).Select(x => new Length(x)).ToList()
            );
        public ParameterSet OriginalParameterSet { get; set; }
        public ParameterSet CurrentParameterSet { get; set; }

        public ReadOnlyCollection<Length> LengthOptions => _lengthOptions;

        private Length _selectedLength;
        public Length SelectedLength
        {
            get { return _selectedLength; }
            set
            {
                _selectedLength = value;
                CurrentParameterSet.Length = _selectedLength.Value;
            }
        }

        public Command SaveCommand { get; private set; }
        public Command CancelCommand { get; private set; }

        public GeneratorOptionsViewModel(GeneratorViewModel parentViewModel)
        {
            Title = "Generation Options";

            CurrentParameterSet = parentViewModel.ParameterSet;
            OriginalParameterSet = CurrentParameterSet.Copy();

            SaveCommand = new Command(ExecuteSaveCommand);
            CancelCommand = new Command(ExecuteCancelCommand);

            SelectedLength = new Length(OriginalParameterSet.Length);
        }

        void ExecuteSaveCommand()
        {
            // do nothing
        }
        void ExecuteCancelCommand()
        {
            CurrentParameterSet.Length = OriginalParameterSet.Length;
            CurrentParameterSet.UnitSets = OriginalParameterSet.UnitSets;
            CurrentParameterSet.IsLegacy = OriginalParameterSet.IsLegacy;
        }
    }
}
