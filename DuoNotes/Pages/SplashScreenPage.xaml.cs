using DuoNotes.Constants;
using DuoNotes.View;

using System;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DuoNotes.Pages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenPage : ContentPage {

        public SplashScreenPage() {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed() {
            return true;
        }
    }
}