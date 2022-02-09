using DuoNotes.Services;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Xamarin.Forms;

using User = DuoNotes.Model.User;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class LoginPageModel {

        readonly FirebaseServices Services;

        public ICommand NavigateToRegisterCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public User User { get; set; }

        public bool IsRegisterAllowed { get; set; }


        public LoginPageModel() {

            User = new User {
                OnAnyPropertiesChanged = () => {

                    (LoginCommand as Command).ChangeCanExecute();
                }
            };

            NavigateToRegisterCommand = new Command(OpenRegisterPopUpAction);

            Services = new FirebaseServices();

            LoginCommand = new Command(LoginAction, CanPreformAction);

        }

        private async void OpenRegisterPopUpAction() {
            await PopupNavigation.Instance.PushAsync(new RegisterPopUp());
        }

        private bool CanPreformAction() {
            return User != null && !string.IsNullOrEmpty(User.Email) &&
                new EmailAddressAttribute().IsValid(User.Email) &&
                !string.IsNullOrEmpty(User.Password);

        }

        private async void LoginAction() {
            await Services.LoginAsync(User);

        }
    }
}
