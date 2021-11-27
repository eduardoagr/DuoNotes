using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Services;

using Firebase.Database;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class MainPageViewModel {

        readonly FirebaseServices Servces;

        readonly string ChildName = "Notebooks";

        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<Notebook> FireBaseNotebooks { get; set; }

        public ObservableCollection<FirebaseObject<NotebookNote>> FirebaseObjects { get; set; }

        public ICommand Logout { get; set; }

        public MainPageViewModel() {

            Servces = new FirebaseServices();

            FirebaseObjects = new ObservableCollection<FirebaseObject<NotebookNote>>();

            FireBaseNotebooks = new ObservableCollection<Notebook>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebookAsync);

            CallNotebookAssync();

            Console.WriteLine(App.UserID);
        }

        private async void NewNotebookAsync() {
            Page mainPage = Application.Current.MainPage;
            string notebookName;
            notebookName = await mainPage.DisplayPromptAsync(
                AppResources.NewNotebook,
                AppResources.NoteBookName);

            if (!string.IsNullOrEmpty(notebookName)) {
                Notebook notebook = new Notebook() {
                    Name = notebookName,
                    UserID = App.UserID,
                    CreatedDate = DateTime.Now
                };

                await Servces.InsertAsync(notebook, ChildName);
            }

            CallNotebookAssync();
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        private async void CallNotebookAssync() {
            var collection = await Servces.ReadAsync(ChildName);
            var NotebookCollection = new List<Notebook>();
            foreach (var item in collection) {

                Notebook notebook = new Notebook {
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = item.Object.Name,
                    CreatedDate = item.Object.CreatedDate,
                };

                Console.WriteLine("Notebooks ID: " + notebook.UserID);
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
