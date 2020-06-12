using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PasswordRegenerator.ViewModels;

namespace PasswordRegenerator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneratorOptionsPage : ContentPage
    {
        GeneratorOptionsViewModel viewModel;

        public GeneratorOptionsPage(GeneratorViewModel parentViewModel)
        {
            InitializeComponent();

            BindingContext = viewModel = new GeneratorOptionsViewModel(parentViewModel);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}