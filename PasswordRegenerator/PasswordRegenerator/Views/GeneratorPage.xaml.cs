using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PasswordRegenerator.Models;
using PasswordRegenerator.Views;
using PasswordRegenerator.ViewModels;

namespace PasswordRegenerator.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class GeneratorPage : ContentPage
    {
        GeneratorViewModel viewModel;

        public GeneratorPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new GeneratorViewModel();
        }

        private async void OptionsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new GeneratorOptionsPage(viewModel)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}