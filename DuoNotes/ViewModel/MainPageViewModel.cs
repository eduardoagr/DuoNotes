using DuoNotes.Fonts;
using DuoNotes.Model;
using DuoNotes.Resources;
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

        readonly string ChildName = "Notebooks";

        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<FirebaseObject<NotebookNote>> Notebooks { get; set; }

        public ICommand Logout { get; set; }


        public MainPageViewModel() {

            Servces = new FirebaseServices();

            Notebooks = new ObservableCollection<FirebaseObject<NotebookNote>>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebookAsync);

            //CallNotebookAssync();

        }

        private async void NewNotebookAsync() {
            string name = await Application.Current.MainPage.DisplayPromptAsync(AppResources.NewNotebook, Resources.AppResources.NoteBookName);
            if (!string.IsNullOrEmpty(name)) {
                Notebook notebook = new Notebook() { Name = name, UserID = App.UserID, CreatedDate = DateTime.Now };
                await Servces.InsertAsync(notebook, ChildName);
            }

            CallNotebookAssync();

            Console.WriteLine(Notebooks.Count);
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        private async void CallNotebookAssync() {
            var collection = await Servces.ReadAsync(ChildName);
            collection = collection.Where(n => ((Notebook)n.Object).UserID == App.UserID).ToList();
            Notebooks.Clear();
            foreach (var item in collection) {
                Notebooks.Add(item);
            }

        }

    }
}
