using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class RegisterPopUpViewModel {

        readonly FirebaseServices Services;

        public ICommand Register { get; set; }

        public User User { get; set; }

        public RegisterPopUpViewModel() {

            User = new User {
                OnAnyPropertiesChanged = () => {

                    (Register as Command).ChangeCanExecute();
                }
            };

            Services = new FirebaseServices();

            Register = new Command(RegisterActionAsync, CanPreformAction);
        }

        private bool CanPreformAction() {

            return User != null && !string.IsNullOrEmpty(User.Email) &&
                new EmailAddressAttribute().IsValid(User.Email) &&
                !string.IsNullOrEmpty(User.Password);

        }

        private async void RegisterActionAsync() {

            await Services.RegisterAsync(User);
        }

    }
}
