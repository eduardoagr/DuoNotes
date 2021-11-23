
using Acr.UserDialogs;

using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Services;
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

        public Model.User Users { get; set; }

        public bool IsRegisterAllowed { get; set; }

        public bool IsPopUpOpen { get; set; }


        FirebaseServices Services;


        public LoginPageViewModel() {

            Users = new Model.User {
                OnAnyPropertiesChanged = () => {

                    (Login as Command).ChangeCanExecute();
                    (Register as Command).ChangeCanExecute();

                }
            };

            Services = new FirebaseServices();

            Login = new Command(LoginAction, CanPreformAction);

            Register = new Command(RegisterActionAsync, CanPreformAction);

            OpenRegisterPopupCommand = new Command(() => {
                IsPopUpOpen = true;
            });
        }

        private bool CanPreformAction() {

            return Users != null && !string.IsNullOrEmpty(Users.Email) &&
                new EmailAddressAttribute().IsValid(Users.Email) &&
                !string.IsNullOrEmpty(Users.Password);

        }

        private async void RegisterActionAsync() {

            await Services.RegisterAsync(Users);
        }
        private async void LoginAction() {

            await Services.LoginAsync(Users);

        }
    }
}
