using DuoNotes.Fonts;
using DuoNotes.Model;
using DuoNotes.Services;

using Firebase.Database;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class MainPageViewModel {

        readonly FirebaseServices Servces;

        readonly Notebook Notebook;

        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<FirebaseObject<Notebook>> Notebooks { get; set; }

        public ICommand Logout { get; set; }


        public MainPageViewModel() {

            Notebook = new Notebook();

            Servces = new FirebaseServices();

            Notebooks = new ObservableCollection<FirebaseObject<Notebook>>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebook);

            CallNotebooks();

        }

        private async void NewNotebook(object obj) {
            string name = await App.Current.MainPage.DisplayPromptAsync(string.Empty, Resources.AppResources.NoteBookName);
            if (!string.IsNullOrEmpty(name)) {
                await Servces.AddNotebook(Notebook, name);
            }

            CallNotebooks();

            Console.WriteLine(Notebooks.Count);
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        private async void CallNotebooks() {
            var collection = await Servces.GetNotebooks();
            Console.WriteLine(App.UserID);
            collection.Where(n => n.Object.UserID == App.UserID).ToList();
            Notebooks.Clear();
            foreach (var item in collection) {
                Notebooks.Add(item);
            }

        }


    }
}
