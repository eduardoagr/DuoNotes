using DuoNotes.Services;

using PropertyChanged;

using System.Collections.Generic;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    [AddINotifyPropertyChangedInterface]
    internal class ProfilePageModel {

        public Command SelectedAvatarCommand { get; set; }

        public Command UpdateCommand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public List<string> Avatars { get; set; }

        public string SelectedAvatar { get; set; }

        public string DisplayName { get; set; }

        public ProfilePageModel() {

            Avatars = AvaarService.GetAvatars();

            SelectedAvatarCommand = new Command(SelectAvatarAction);

            UpdateCommand = new Command(SaveProfile);

            GetUserData();
        }

        private async void SaveProfile() {
            DisplayName = FireUser.DisplayName;
            if (!string.IsNullOrEmpty(SelectedAvatar) || !string.IsNullOrEmpty(DisplayName)) {
                FireUser = await App.FirebaseService.UpdateUserDataAsync(SelectedAvatar, DisplayName);
            } else {
                GetUserData();
            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void SelectAvatarAction() {
            if (SelectedAvatar == null) {
                return;
            }
        }

        private async void GetUserData() {
            FireUser = await App.FirebaseService.GetProfileInformationAndRefreshTokenAsync();
            SelectedAvatar = FireUser.PhotoUrl;
        }

    }
}
