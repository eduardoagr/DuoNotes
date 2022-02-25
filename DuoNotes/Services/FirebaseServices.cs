using Acr.UserDialogs;

using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Utils;
using DuoNotes.View;

using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

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
        private readonly FirebaseClient firebaseClient;
        const string BASE_URL = "https://duonotes-f2b77-default-rtdb.europe-west1.firebasedatabase.app/";
        
        public FirebaseServices() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(AppConstant.WEB_API_KEY));
            firebaseClient = new FirebaseClient(BASE_URL);
        }

        
        public async Task RegisterAsync(User users) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(users.Email, users.Password);
                await PopupNavigation.Instance.PopAsync(true);
                Application.Current.MainPage = new NavigationPage(new NotebooksPage());
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
                Preferences.Set(AppConstant.FirebaseToken, auth.FirebaseToken);
                Preferences.Set(AppConstant.UserID, auth.User.LocalId);
                Preferences.Set(AppConstant.FirebaseRefreshToken, serializedcontnet);
                UserDialogs.Instance.HideLoading();
                Application.Current.MainPage = new NavigationPage(new NotebooksPage());
            } catch (FirebaseAuthException ex) {
                Firebasemessages.GetMessages(ex);
            }
            UserDialogs.Instance.HideLoading();
        }

        
        public void LogOut() {
            Preferences.Clear();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        
        public async Task<Firebase.Auth.User> GetProfileInformationAndRefreshTokenAsync() {

            var savedfirebaseauth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get(AppConstant.FirebaseRefreshToken, string.Empty));
            var RefreshedContent = await AuthProvider.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set(AppConstant.FirebaseRefreshToken, JsonConvert.SerializeObject(RefreshedContent));

            if (string.IsNullOrEmpty(savedfirebaseauth.User.PhotoUrl) ||
                string.IsNullOrEmpty(savedfirebaseauth.User.DisplayName)) {

                savedfirebaseauth.User.PhotoUrl = "msn.svg";
                savedfirebaseauth.User.DisplayName = AppResources.User;
            }

            return savedfirebaseauth.User;

        }

        
        public async Task<Firebase.Auth.User> UpdateUserDataAsync(string PhotoUri, string DisplyName) {

            var savedfirebaseauth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get(AppConstant.FirebaseRefreshToken,
                string.Empty));

            var newUser = await AuthProvider.UpdateProfileAsync(savedfirebaseauth.FirebaseToken, DisplyName,
                PhotoUri);

            savedfirebaseauth.User.DisplayName = string.IsNullOrEmpty(DisplyName) ? savedfirebaseauth.User.DisplayName : DisplyName;
            savedfirebaseauth.User.PhotoUrl = string.IsNullOrEmpty(PhotoUri) ? savedfirebaseauth.User.PhotoUrl : PhotoUri;

            var RefreshedContent = await AuthProvider.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set(AppConstant.FirebaseRefreshToken, JsonConvert.SerializeObject(RefreshedContent));

            return newUser.User;
        }

        
        public async Task InsertAsync(NotebookNote element, string ChildName) {
            if (element == null || ChildName == null) {
                return;
            }
            await firebaseClient.Child(ChildName)
                .PostAsync(JsonConvert.SerializeObject(element));
        }

        
        public async Task<ObservableCollection<NotebookNote>> ReadAsync(string ChildName, string NotebookId = "") {

            var list = await firebaseClient.Child(ChildName)
                 .OnceAsync<NotebookNote>();

            var collection = new List<NotebookNote>();

            foreach (var item in list) {
                NotebookNote notebookNote = null;
                notebookNote = Convert(ChildName, item);
                collection.Add(notebookNote);
            }

            if (ChildName.Equals(AppConstant.Notes)) {
                collection = collection.Where(n => ((Note)n).NotebookId == NotebookId).ToList();
            } else {
                collection = collection.Where(n => n.UserID == Preferences.Get(AppConstant.UserID, string.Empty)).ToList();
            }
            FireBaseNotebooks.Clear();
            foreach (var element in collection) {
                FireBaseNotebooks.Add(element);
            }

            return FireBaseNotebooks;
        }


        //public async Task<Note> ReadNoteAsync(string Id) {

        //    var allPersons = await GetAllPersons();
        //    await firebase
        //      .Child("Persons")
        //      .OnceAsync<Person>();
        //    return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();

        //}

        public async void UpdateNotebookNoten(string Id) {
            //TODO: Update the Note file location
        }

        public async void DeleteNotebookNote(string Id, string ChildName) {
                
             await firebaseClient
                  .Child(ChildName)
                  .Child(Id)
                  .DeleteAsync();

            //TODO delete the notes of that notebook
            
        }

        //This method will convert whatever we passed, to a specific object, based on the childname
        private static NotebookNote Convert(string ChildName, FirebaseObject<NotebookNote> item) {
            NotebookNote notebookNote;
            if (ChildName.Equals(AppConstant.Notebooks)) {
                notebookNote = new Notebook {
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = $"{AppResources.NoteBookName}: {item.Object.Name}",
                    CreatedDate = item.Object.CreatedDate,
                    Color = item.Object.Color,
                };
            } else {
                notebookNote = new Note {
                    NotebookId = item.Object.NotebookId,
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = $"{AppResources.NoteTite}: {item.Object.Name}",
                    CreatedDate = item.Object.CreatedDate,
                    Color = item.Object.Color,
                };
            }

            return notebookNote;
        }
    }
}
