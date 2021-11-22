using Acr.UserDialogs;

using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.View;

using Firebase.Auth;
using Firebase.Database;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

using User = DuoNotes.Model.User;

namespace DuoNotes.Services {
    public class FirebaseServices {

        FirebaseAuthProvider authProvider;
        FirebaseClient Client;
        string childName = "Notebooks";

        public FirebaseServices() {

            authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.WEB_API_KEY));
            Client = new FirebaseClient("https://duonotes-f2b77-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public async Task RegisterAsync(User users) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(users.Email, users.Password);
                await Application.Current.MainPage.DisplayAlert(
                AppResources.NewUser,
              AppResources.UserInserted, "OK");
                UserDialogs.Instance.HideLoading();
            } catch (FirebaseAuthException ex) {
                GetErrorMessage(ex);
            }
            UserDialogs.Instance.HideLoading();
        }


        public async Task LoginAsync(User users) {

            try {
                UserDialogs.Instance.ShowLoading(AppResources.Loading);
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(users.Email, users.Password);
                Preferences.Set(App.UID, auth.User.LocalId);
                App.UserID = auth.User.LocalId;
                UserDialogs.Instance.HideLoading();
                Application.Current.MainPage = new NavigationPage(new MainPage());
            } catch (FirebaseAuthException ex) {
                GetErrorMessage(ex);
            }
            UserDialogs.Instance.HideLoading();
        }

        public async Task AddNotebook(Notebook notebook, string name) {
            if (notebook is null) {
                throw new ArgumentNullException(nameof(notebook));
            }
            notebook = new Notebook() { Id = notebook.Id, Name = name, UserID = App.UserID, CreatedDate = DateTime.Today.Date };
            await Client.Child(childName).PostAsync(JsonConvert.SerializeObject(notebook));
        }

        public async Task<ObservableCollection<FirebaseObject<Notebook>>> GetNotebooks() {

            var NotebooksCollection = new ObservableCollection<FirebaseObject<Notebook>>();

            var list = await Client.Child(childName).OnceAsync<Notebook>();

            foreach (var item in list) {
                item.Object.Id = item.Key;
                NotebooksCollection.Add(item);
            }

            return NotebooksCollection;
        }

        public void LogOut() {
            Preferences.Clear();
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public void GetErrorMessage(FirebaseAuthException ex) {

            var stringError = JsonConvert.DeserializeObject<Response>(ex.ResponseData);

            switch (stringError.Error.Message) {
                case "EMAIL_EXISTS":
                    App.Current.MainPage.DisplayAlert(AppResources.
                        ServerError, AppResources.EMAIL_EXISTS,
                        AppResources.OK);
                    break;

                case "WEAK_PASSWORD : Password should be at least 6 characters":
                    App.Current.MainPage.DisplayAlert(
                        AppResources.ServerError,
                        AppResources.WEAK_PASSWORD___Password_should_be_at_least_6_characters,
                        AppResources.OK);
                    break;

                case "EMAIL_NOT_FOUND":
                    App.Current.MainPage.DisplayAlert(
                       AppResources.ServerError,
                       AppResources.EMAIL_NOT_FOUND,
                       AppResources.OK);
                    break;

                default:
                    break;
            }
        }
    }


}
