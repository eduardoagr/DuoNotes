using DuoNotes.Constants;
using DuoNotes.View;

using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    internal class SplashScreenPageModel : Page {

        public Command FinishedAnimtionCommand { get; set; }

        public SplashScreenPageModel() {

            FinishedAnimtionCommand = new Command(OnFinishedAction);
        }

        private void OnFinishedAction() {
            var UserID = Preferences.Get(AppConstant.UserID, string.Empty);

            if (!string.IsNullOrEmpty(UserID)) {
                Application.Current.MainPage = new NavigationPage(new NotebooksPage());
            } else {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }
    }
}
