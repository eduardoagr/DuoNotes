using DuoNotes.Services;

using PropertyChanged;

using System;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    internal class ProfilePageViewModel {


        readonly FirebaseServices Services;

        public ICommand SelectedAvatarCommand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public List<string> Avatars { get; set; }

        public string SelectedAvatar { get; set; }

        public string DisplaName { get; set; }

        public Uri AvatarUri { get; set; }

        public ProfilePageViewModel() {

            App.services = new FirebaseServices();

            Avatars = AvaarServices.GetAvatars();

            SelectedAvatarCommand = new Command(SelectAvatarAction);

            GetUserData();
        }

        private void SelectAvatarAction() {
            throw new NotImplementedException();
        }

        private async void GetUserData() {
            FireUser = await App.services.GetProfileInformationAndRefreshToken();
        }

    }
}
