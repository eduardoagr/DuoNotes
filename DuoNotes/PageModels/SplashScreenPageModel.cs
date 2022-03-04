using DuoNotes.Constants;
using DuoNotes.View;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels {
    internal class SplashScreenPageModel {

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
    }
}
