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

        public ObservableCollection<FirebaseObject<IElementProperties>> FirebaseObjects { get; set; }

        public ObservableCollection<Notebook> Notebooks { get; set; }

        public ICommand Logout { get; set; }

        public MainPageViewModel() {

            Servces = new FirebaseServices();

            FirebaseObjects = new ObservableCollection<FirebaseObject<IElementProperties>>();

            Notebooks = new ObservableCollection<Notebook>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebookAsync);

        }

        private async void NewNotebookAsync() {
            string NotebookName = await Application.Current.MainPage.DisplayPromptAsync(AppResources.NewNotebook, AppResources.NoteBookName);
            if (!string.IsNullOrEmpty(NotebookName)) {
                Notebook notebook = new Notebook() { Name = NotebookName, UserID = App.UserID, YearOfCreation = DateTime.Now.ToString("yyyy") };
                await Servces.InsertAsync(notebook, ChildName);
            }

            CallNotebookAssync();
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        private async void CallNotebookAssync() {
            var collection = await Servces.ReadAsync(ChildName);
            var tempList = new ObservableCollection<Notebook>();
            foreach (var item in collection) {
                //Notebook notebook = new Notebook {
                //    Id = item.Object.Id,
                //    Name = item.Object.Name,
                //    YearOfCreation = item.Object.YearOfCreation,
                //    UserID = item.Object.user
            };
            //tempList.Add(notebook);
        }
        //Notebooks.Clear();
        //foreach (var item in tempList) {
        //    if (item.UserID == ) {

        //    }
        //    Notebooks.Add(item);
        //}
    }
}