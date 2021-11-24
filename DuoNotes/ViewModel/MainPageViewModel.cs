using DuoNotes.Fonts;
using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Services;

using Firebase.Database;

using System;
using System.Collections.Generic;
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

        public ObservableCollection<FirebaseObject<NotebookNote>> firebaseObjects { get; set; }

        public ObservableCollection<Notebook> Notebooks { get; set; }

        public ICommand Logout { get; set; }


        public MainPageViewModel() {

            Servces = new FirebaseServices();

            firebaseObjects = new ObservableCollection<FirebaseObject<NotebookNote>>();

            Notebooks = new ObservableCollection<Notebook>();


            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebookAsync);

            CallNotebookAssync();

        }

        private async void NewNotebookAsync() {
            string NotebookName = await Application.Current.MainPage.DisplayPromptAsync(AppResources.NewNotebook, AppResources.NoteBookName);
            if (!string.IsNullOrEmpty(NotebookName)) {
                Notebook notebook = new Notebook() { Name = NotebookName, UserID = App.UserID, CreatedDate = DateTime.Now.ToString("yyyy") };
                await Servces.InsertAsync(notebook, ChildName);
            }

            CallNotebookAssync();
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        private async void CallNotebookAssync() {
            var collection = await Servces.ReadAsync(ChildName);
            foreach (var item in collection) {
                Notebook notebook = new Notebook();
                notebook.Id = item.Object.Id;
                notebook.Name = item.Object.Name;
                notebook.CreatedDate = DateTime.Now.ToString("yyyy");
                notebook.UserID = App.UserID;
                Notebooks.Add(notebook);
            }

            var tempList = new List<Notebook>();

            tempList = Notebooks.Where(n => n.UserID == App.UserID).ToList();
            Notebooks.Clear();
            foreach (var item in tempList) {
                Notebooks.Add(item);
            }

        }
    }
}