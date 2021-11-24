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

        public ObservableCollection<Notebook> NewNotebooksObjects { get; set; }

        public ObservableCollection<FirebaseObject<NotebookNote>> FirebaseObjects { get; set; }

        public ICommand Logout { get; set; }


        public MainPageViewModel() {

            Servces = new FirebaseServices();

            FirebaseObjects = new ObservableCollection<FirebaseObject<NotebookNote>>();

            NewNotebooksObjects = new ObservableCollection<Notebook>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebookAsync);

            CallNotebookAssync();

        }

        private async void NewNotebookAsync() {
            string name = await Application.Current.MainPage.DisplayPromptAsync(AppResources.NewNotebook, AppResources.NoteBookName);
            if (!string.IsNullOrEmpty(name)) {
                Notebook notebook = new Notebook() { Name = name, UserID = App.UserID, CreatedDate = DateTime.Now.ToString("yyyy") };
                await Servces.InsertAsync(notebook, ChildName);
            }

            CallNotebookAssync();
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        private async void CallNotebookAssync() {
            var collection = await Servces.ReadAsync(ChildName);
            ObservableCollection<Notebook> NotebookCollection = new ObservableCollection<Notebook>();
            foreach (var item in collection) {

                Notebook notebook = new Notebook {
                    UserID = App.UserID,
                    Id = item.Key,
                    Name = item.Object.Name,
                    CreatedDate = DateTime.Now.ToShortDateString(),
                };
                NotebookNote notebookNote = item.Object as NotebookNote;
                notebook.Id = notebookNote.Id;
                notebook.Name = notebookNote.Name;
                notebook.CreatedDate = notebookNote.CreatedDate;
                NotebookCollection.Add(notebook);
            }

             = NotebookCollection.Where(n => n.UserID == App.UserID).ToList();

            NewNotebooksObjects.Clear();
            foreach (var element in NewNotebooksObjects) {
                NewNotebooksObjects.Add(element);
            }

        }
    }

}
