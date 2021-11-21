using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class MainPageViewModel {

        FirebaseServices Servces;

        Notebook Notebook;

        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<Notebook> Notebooks { get; set; }

        public ICommand Logout { get; set; }


        public MainPageViewModel() {

            Notebook = new Notebook();

            Servces = new FirebaseServices();

            Notebooks = new ObservableCollection<Notebook>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(NewNotebook);
        }

        private async void NewNotebook(object obj) {
            string name = await App.Current.MainPage.DisplayPromptAsync("", Resources.AppResources.NoteBookName);
            if (!string.IsNullOrEmpty(name)) {
                await Servces.AddNotebook(Notebook, name);
            }
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }
    }
}
