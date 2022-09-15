using System.ComponentModel.DataAnnotations;

using DuoNotes.Model;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps
{

    [AddINotifyPropertyChangedInterface]
    public class RegisterPopUpPageModel
    {

        public Command RegisterCommand
        {
            get; set;
        }

        public Command CloseCommand
        {
            get; set;
        }

        public User User
        {
            get; set;
        }

        public RegisterPopUpPageModel()
        {

            User = new User
            {
                OnAnyPropertiesChanged = () =>
                {

                    RegisterCommand.ChangeCanExecute();
                }
            };

            RegisterCommand = new Command(RegisterActionAsync, CanPreformAction);

            CloseCommand = new Command(PerformCloseAction);
        }

        private bool CanPreformAction()
        {

            return User != null && !string.IsNullOrEmpty(User.Email) &&
                new EmailAddressAttribute().IsValid(User.Email) &&
                !string.IsNullOrEmpty(User.Password);

        }

        private async void RegisterActionAsync()
        {

            await App.FirebaseService.RegisterAsync(User);
        }

        private async void PerformCloseAction()
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
