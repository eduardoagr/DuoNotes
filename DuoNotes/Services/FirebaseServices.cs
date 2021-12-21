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
                Application.Current.MainPage = new NavigationPage(new MainPage());
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
                App.UserID = auth.User.LocalId;
                Preferences.Set(App.UID, App.UserID);
                Preferences.Set(App.FirebaseToken, auth.FirebaseToken);
                UserDialogs.Instance.HideLoading();
                Application.Current.MainPage = new NavigationPage(new MainPage());
            } catch (FirebaseAuthException ex) {
                Firebasemessages.GetMessages(ex);
            }
            UserDialogs.Instance.HideLoading();
        }

        public async Task<Firebase.Auth.User> GetPrefileAsync() {

            var FirebaseUser = await AuthProvider.GetUserAsync(Preferences.Get(App.FirebaseToken, string.Empty));
            return FirebaseUser;
        }

        public void LogOut() {
            Preferences.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public async Task InsertAsync(NotebookNote element, string ChildName) {
            if (element is null) {
                return;
            }
            await Client.Child(ChildName)
                .PostAsync(JsonConvert.SerializeObject(element));
        }

        public async void ReadAsync(string ChildName) {

            var list = await Client.Child(ChildName)
                 .OnceAsync<NotebookNote>();

            var collection = new List<NotebookNote>();

            foreach (var item in list) {
                NotebookNote notebookNote = null;
                notebookNote = Convert(ChildName, item);
                collection.Add(notebookNote);
            }

            collection = collection.Where(n => n.UserID == App.UserID).ToList();

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
                    Desc = item.Object.Desc
                };
            }

            return notebookNote;
        }
    }
}
