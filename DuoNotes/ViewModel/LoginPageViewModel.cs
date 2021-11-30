using DuoNotes.Services;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Xamarin.Forms;

using User = DuoNotes.Model.User;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class LoginPageViewModel {

        readonly FirebaseServices Services;

        public ICommand NavigateToRegister { get; set; }

        public ICommand Login { get; set; }

        public User User { get; set; }

        public bool IsRegisterAllowed { get; set; }


        public LoginPageViewModel() {

            User = new User {
                OnAnyPropertiesChanged = () => {

                    (Login as Command).ChangeCanExecute();
                }
            };

            NavigateToRegister = new Command(OpenRegisterPopUp);

            Services = new FirebaseServices();

            Login = new Command(LoginAction, CanPreformAction);

        }

        private async void OpenRegisterPopUp() {
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
