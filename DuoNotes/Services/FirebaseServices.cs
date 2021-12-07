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

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }
        readonly FirebaseAuthProvider AuthProvider;
        readonly FirebaseClient Client;
        readonly string BASE_URL = "https://duonotes-f2b77-default-rtdb.europe-west1.firebasedatabase.app/";

        public FirebaseServices(ObservableCollection<NotebookNote> notebookNotes = null) {

            FireBaseNotebooks = notebookNotes;
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(App.WEB_API_KEY));
            Client = new FirebaseClient(BASE_URL);
        }

        public async Task RegisterAsync(User users) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(users.Email, users.Password);
                await Application.Current.MainPage.DisplayAlert(
                AppResources.NewUser,
              AppResources.UserInserted, "OK");
                UserDialogs.Instance.HideLoading();
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

            var NotebookCollection = new List<Notebook>();

            foreach (var item in list) {

                Notebook notebook = new Notebook {
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = item.Object.Name,
                    CreatedDate = item.Object.CreatedDate,
                    Color = item.Object.Color,
                    Desc = item.Object.Desc
                };
                NotebookCollection.Add(notebook);
            }

            NotebookCollection = NotebookCollection.Where(n => n.UserID == App.UserID).ToList();

            FireBaseNotebooks.Clear();
            foreach (var element in NotebookCollection) {
                FireBaseNotebooks.Add(element);
            }

        }
    }
}
