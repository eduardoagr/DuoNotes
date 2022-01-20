using DuoNotes.Services;

using PropertyChanged;

using System;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    internal class ProfilePageViewModel {

        public ICommand SelectedAvatarCommand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public List<string> Avatars { get; set; }

        public string SelectedAvatar { get; set; }

        public string DisplayName { get; set; }

        public ProfilePageViewModel() {

            App.services = new FirebaseServices();

            Avatars = AvaarServices.GetAvatars();

            SelectedAvatarCommand = new Command(SelectAvatarAction);

            GetUserData();
        }

        private async void SelectAvatarAction() {

            await App.services.UpdateUserData(SelectedAvatar, DisplayName);

            Console.WriteLine(FireUser.PhotoUrl);
        }

        private async void GetUserData() {
            FireUser = await App.services.GetProfileInformationAndRefreshToken();
            if (string.IsNullOrEmpty(FireUser.DisplayName)) {
                DisplayName = Resources.AppResources.User;
            } else {
                DisplayName = FireUser.DisplayName;
            }
        }

    }
}
