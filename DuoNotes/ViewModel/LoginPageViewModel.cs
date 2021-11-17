
using Acr.UserDialogs;

using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Utils;

using Firebase.Auth;

using PropertyChanged;

using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class LoginPageViewModel {

        public ICommand OpenRegisterPopupCommand { get; set; }

        public ICommand Register { get; set; }

        public ICommand Login { get; set; }

        public Users Users { get; set; }

        public bool IsRegisterAllowed { get; set; }

        public bool IsPopUpOpen { get; set; }


        FirebaseAuthProvider authProvider;


        public LoginPageViewModel() {

            Users = new Users {
                OnAnyPropertiesChanged = () => {

                    (Login as Command).ChangeCanExecute();
                    (Register as Command).ChangeCanExecute();

                }
            };

            authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.WEB_API_KEY));

            Login = new Command(LoginAction, CanPreformAction);

            Register = new Command(RegisterActionAsync, CanPreformAction);

            OpenRegisterPopupCommand = new Command(() => {
                IsPopUpOpen = true;
            });
        }

        private bool CanPreformAction(object arg) {

            return Users != null && !string.IsNullOrEmpty(Users.Email) &&
                new EmailAddressAttribute().IsValid(Users.Email) &&
                !string.IsNullOrEmpty(Users.Password);

        }

        private async void RegisterActionAsync(object obj) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Users.Email, Users.Password);
                await Application.Current.MainPage.DisplayAlert(
                AppResources.NewUser,
              AppResources.UserInserted, "OK");
                UserDialogs.Instance.HideLoading();
            } catch (FirebaseAuthException ex) {
                Exceptions.GetErrorMessage(ex);
            }
            UserDialogs.Instance.HideLoading();





        }
        private async void LoginAction(object obj) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Users.Email, Users.Password);
                Preferences.Set(App.UID, auth.User.LocalId);
                App.UserID = auth.User.LocalId;
                UserDialogs.Instance.HideLoading();
                Application.Current.MainPage = new NavigationPage(new MainPage());
            } catch (FirebaseAuthException ex) {
                Exceptions.GetErrorMessage(ex);
            }
            UserDialogs.Instance.HideLoading();
        }
    }
}
