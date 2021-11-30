using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View.PopUps;

using Rg.Plugins.Popup.Services;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class MainPageViewModel {

        readonly FirebaseServices services;


        readonly string ChildName = "Notebooks";

        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public ICommand Logout { get; set; }

        public MainPageViewModel() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            services = new FirebaseServices(FireBaseNotebooks);

            Logout = new Command(LogOut);

            CreateNotebook = new Command(OpenCreateNewNotebookPopUp);

            services.ReadAsync(ChildName);
        }

        private async void OpenCreateNewNotebookPopUp() {
            await PopupNavigation.Instance.PushAsync(new NotebookPopUp());
        }

        private void LogOut(object obj) {
            services.LogOut();
        }

        //public async void FirebseReadAsync() {
        //     var collection = await Servces.ReadAsync(ChildName);
        //     var NotebookCollection = new List<Notebook>();
        //     foreach (var item in collection) {

        //         Notebook notebook = new Notebook {
        //             UserID = item.Object.UserID,
        //             Id = item.Key,
        //             Name = item.Object.Name,
        //             CreatedDate = item.Object.CreatedDate,
        //             Color = item.Object.Color,
        //             Desc = item.Object.Desc
        //         };
        //         NotebookCollection.Add(notebook);
        //     }

        //     NotebookCollection = NotebookCollection.Where(n => n.UserID == App.UserID).ToList();

        //     FireBaseNotebooks.Clear();
        //     foreach (var element in NotebookCollection) {
        //         FireBaseNotebooks.Add(element);
        //     }

        // }
    }

}
