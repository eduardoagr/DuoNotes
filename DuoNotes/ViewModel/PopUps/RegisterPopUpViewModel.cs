using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class RegisterPopUpViewModel {

        readonly FirebaseServices Services;

        public ICommand Register { get; set; }

        public ICommand Close { get; set; }

        public User User { get; set; }

        public RegisterPopUpViewModel() {

            User = new User {
                OnAnyPropertiesChanged = () => {

                    (Register as Command).ChangeCanExecute();
                }
            };

            Services = new FirebaseServices();

            Register = new Command(RegisterActionAsync, CanPreformAction);

            Close = new Command(PerformClose);
        }

        private bool CanPreformAction() {

            return User != null && !string.IsNullOrEmpty(User.Email) &&
                new EmailAddressAttribute().IsValid(User.Email) &&
                !string.IsNullOrEmpty(User.Password);

        }

        private async void RegisterActionAsync() {

            await Services.RegisterAsync(User);
        }

        private async void PerformClose() {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
