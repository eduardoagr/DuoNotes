using DuoNotes.Constants;
using DuoNotes.Pages;
using DuoNotes.View;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels
{
    internal class SplashScreenPageModel
    {

        public Command FinishedAnimtionCommand
        {
            get; set;
        }

        public SplashScreenPageModel()
        {

            FinishedAnimtionCommand = new Command(OnFinishedAction);
        }

        private void OnFinishedAction()
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {

                var UserID = Preferences.Get(AppConstant.UserID, string.Empty);

                if (!string.IsNullOrEmpty(UserID))
                {
                    Application.Current.MainPage = new NavigationPage(new NotebooksPage());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new NoDataPage());
            }

        }
    }
}
