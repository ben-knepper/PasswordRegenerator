using System;
using System.Collections.Generic;
using System.Text;

using PasswordGeneration;
using PasswordRegenerator.Models;
using Xamarin.Forms;

namespace PasswordRegenerator.ViewModels
{
    class GeneratorOptionsViewModel : BaseViewModel
    {
        public ParameterSet OriginalParameterSet { get; set; }
        public ParameterSet CurrentParameterSet { get; set; }

        public Command SaveCommand { get; private set; }
        public Command CancelCommand { get; private set; }

        public GeneratorOptionsViewModel(GeneratorViewModel parentViewModel)
        {
            Title = "Generation Options";

            CurrentParameterSet = parentViewModel.ParameterSet;
            OriginalParameterSet = CurrentParameterSet.Copy();

            SaveCommand = new Command(ExecuteSaveCommand);
            CancelCommand = new Command(ExecuteCancelCommand);
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
