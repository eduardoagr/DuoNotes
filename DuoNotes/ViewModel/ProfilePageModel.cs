using DuoNotes.Services;

using PropertyChanged;

using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    internal class ProfilePageModel {

        readonly FirebaseServices Services;

        public ICommand SelectedAvatarCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public List<string> Avatars { get; set; }

        public string SelectedAvatar { get; set; }

        public string DisplayName { get; set; }

        public ProfilePageModel() {

            Services = App.services;

            Avatars = AvaarServices.GetAvatars();

            SelectedAvatarCommand = new Command(SelectAvatarAction);

            UpdateCommand = new Command(SaveProfile);

            GetUserData();
        }

        private async void SaveProfile() {
            if (!string.IsNullOrEmpty(SelectedAvatar) || string.IsNullOrEmpty(DisplayName)) {
                FireUser = await App.services.UpdateUserData(SelectedAvatar, DisplayName);
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
            FireUser = await App.services.GetProfileInformationAndRefreshToken();
            SelectedAvatar = FireUser.PhotoUrl;
        }

    }
}
