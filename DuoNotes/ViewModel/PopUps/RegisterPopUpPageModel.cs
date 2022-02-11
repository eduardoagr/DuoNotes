﻿using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class RegisterPopUpPageModel {

        public ICommand RegisterCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public User User { get; set; }

        public RegisterPopUpPageModel() {

            User = new User {
                OnAnyPropertiesChanged = () => {

                    (RegisterCommand as Command).ChangeCanExecute();
                }
            };

            RegisterCommand = new Command(RegisterActionAsync, CanPreformAction);

            CloseCommand = new Command(PerformCloseAction);
        }

        private bool CanPreformAction() {

            return User != null && !string.IsNullOrEmpty(User.Email) &&
                new EmailAddressAttribute().IsValid(User.Email) &&
                !string.IsNullOrEmpty(User.Password);

        }

        private async void RegisterActionAsync() {

            await App.FirebaseServices.RegisterAsync(User);
        }

        private async void PerformCloseAction() {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
