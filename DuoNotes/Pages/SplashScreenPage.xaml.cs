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

        private void AnimationView_OnFinishedAnimation(object sender, EventArgs e) {

            var UserID = Preferences.Get(AppConstant.UserID, string.Empty);

            if (!string.IsNullOrEmpty(UserID)) {
                Application.Current.MainPage = new NavigationPage(new NotebooksPage());
            } else {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        // This is only to make sure the user do not press the back button
        protected override bool OnBackButtonPressed() {
            return true;
        }
    }
}