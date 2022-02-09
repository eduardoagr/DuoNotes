using Acr.UserDialogs;

using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Utils;
using DuoNotes.View;

using Firebase.Auth;
using Firebase.Database;

using Newtonsoft.Json;

using Rg.Plugins.Popup.Services;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

using User = DuoNotes.Model.User;

namespace DuoNotes.Services {
    public class FirebaseServices {

        private ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }
        private readonly FirebaseAuthProvider AuthProvider;
        private readonly FirebaseClient Client;
        const string BASE_URL = "https://duonotes-f2b77-default-rtdb.europe-west1.firebasedatabase.app/";
        public FirebaseServices(ObservableCollection<NotebookNote> notebookNotes = null) {

            FireBaseNotebooks = notebookNotes;
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(App.WEB_API_KEY));
            Client = new FirebaseClient(BASE_URL);
        }

        public async Task RegisterAsync(User users) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(users.Email, users.Password);
                await App.Current.MainPage.DisplayAlert(string.Empty, AppResources.UserInserted, "OK");
                await PopupNavigation.Instance.PopAsync(true);
            } catch (FirebaseAuthException ex) {
                Firebasemessages.GetMessages(ex);
            }
            UserDialogs.Instance.HideLoading();
        }

        public async Task LoginAsync(User users) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await AuthProvider.SignInWithEmailAndPasswordAsync(users.Email, users.Password);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set(App.FirebaseToken, auth.FirebaseToken);
                Preferences.Set(App.UserID, auth.User.LocalId);
                Preferences.Set(App.FirebaseRefreshToken, serializedcontnet);
                UserDialogs.Instance.HideLoading();
                Application.Current.MainPage = new NavigationPage(new MainPage());
            } catch (FirebaseAuthException ex) {
                Firebasemessages.GetMessages(ex);
            }
            UserDialogs.Instance.HideLoading();
        }

        public void LogOut() {
            Preferences.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public async Task<Firebase.Auth.User> GetProfileInformationAndRefreshToken() {

            var savedfirebaseauth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get(App.FirebaseRefreshToken, string.Empty));
            var RefreshedContent = await AuthProvider.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set(App.FirebaseRefreshToken, JsonConvert.SerializeObject(RefreshedContent));

            if (string.IsNullOrEmpty(savedfirebaseauth.User.PhotoUrl) ||
                string.IsNullOrEmpty(savedfirebaseauth.User.DisplayName)) {

                savedfirebaseauth.User.PhotoUrl = "msn.svg";
                savedfirebaseauth.User.DisplayName = AppResources.User;
            }

            return savedfirebaseauth.User;

        }

        public async Task<Firebase.Auth.User> UpdateUserData(string PhotoUri, string DisplyName) {

            var savedfirebaseauth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get(App.FirebaseRefreshToken,
                string.Empty));

            var newUser = await AuthProvider.UpdateProfileAsync(savedfirebaseauth.FirebaseToken, DisplyName,
                PhotoUri);

            savedfirebaseauth.User.DisplayName = string.IsNullOrEmpty(DisplyName) ? savedfirebaseauth.User.DisplayName : DisplyName;
            savedfirebaseauth.User.PhotoUrl = string.IsNullOrEmpty(PhotoUri) ? savedfirebaseauth.User.PhotoUrl : PhotoUri;

            var RefreshedContent = await AuthProvider.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set(App.FirebaseRefreshToken, JsonConvert.SerializeObject(RefreshedContent));
            return newUser.User;
        }

        public async Task InsertAsync(NotebookNote element, string ChildName) {
            if (element == null || ChildName == null) {
                return;
            }
            await Client.Child(ChildName)
                .PostAsync(JsonConvert.SerializeObject(element));
        }

        public async void ReadAsync(string ChildName, string NotebookId = "") {

            var list = await Client.Child(ChildName)
                 .OnceAsync<NotebookNote>();

            var collection = new List<NotebookNote>();

            foreach (var item in list) {
                NotebookNote notebookNote = null;
                notebookNote = Convert(ChildName, item);
                collection.Add(notebookNote);
            }

            if (ChildName.Equals(App.Notes)) {
                collection = collection.Where(n => ((Note)n).NotebookId == NotebookId).ToList();
            } else {
                collection = collection.Where(n => n.UserID == Preferences.Get(App.UserID, string.Empty)).ToList();
            }
            FireBaseNotebooks.Clear();
            foreach (var element in collection) {
                FireBaseNotebooks.Add(element);
            }

        }

        //This method will convert whatever we passed, to a specific object, based on the childname
        private static NotebookNote Convert(string ChildName, FirebaseObject<NotebookNote> item) {
            NotebookNote notebookNote;
            if (ChildName.Equals(App.Notebooks)) {
                notebookNote = new Notebook {
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = item.Object.Name,
                    CreatedDate = item.Object.CreatedDate,
                    Color = item.Object.Color,
                    Desc = item.Object.Desc
                };
            } else {
                notebookNote = new Note {
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = item.Object.Name,
                    CreatedDate = item.Object.CreatedDate,
                    Color = item.Object.Color,
                    Desc = item.Object.Desc,
                };
            }

            return notebookNote;
        }
    }
}
