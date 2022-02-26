using DuoNotes.Constants;
using DuoNotes.Services;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Xamarin.Forms;

using User = DuoNotes.Model.User;

namespace DuoNotes.PageModels {

    [AddINotifyPropertyChangedInterface]
    public class LoginPageModel {

        public Command NavigateToRegisterCommand { get; set; }

        public Command LoginCommand { get; set; }

        public User User { get; set; }

        public bool IsRegisterAllowed { get; set; }

        public LoginPageModel() {

            User = new User {
                OnAnyPropertiesChanged = () => {

                    (LoginCommand as Command).ChangeCanExecute();
                }
            };

            NavigateToRegisterCommand = new Command(OpenRegisterPopUpAction);

            LoginCommand = new Command(LoginAction, CanPreformAction);

        }

        private async void OpenRegisterPopUpAction() {
            await PopupNavigation.Instance.PushAsync(new RegisterPopUp(), true);
        }

        private bool CanPreformAction() {
            return User != null && !string.IsNullOrEmpty(User.Email) &&
                new EmailAddressAttribute().IsValid(User.Email) &&
                !string.IsNullOrEmpty(User.Password);

        }

        private async void LoginAction() {
            await App.FirebaseServices.LoginAsync(User);

        }
    }
}
