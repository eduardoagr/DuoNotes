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

        readonly FirebaseServices Servces;

        readonly string ChildName = "Notebooks";

        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<Notebook> FireBaseNotebooks { get; set; }

        public ICommand Logout { get; set; }

        public MainPageViewModel() {

            Servces = new FirebaseServices();

            FireBaseNotebooks = new ObservableCollection<Notebook>();

            Logout = new Command(LogOut);

            CreateNotebook = new Command(OpenCreateNewNotebookPopUp);

            CallNotebookAsync();

        }

        private async void OpenCreateNewNotebookPopUp() {
            await PopupNavigation.Instance.PushAsync(new NotebookPopUp(), true);
        }

        private void LogOut(object obj) {
            Servces.LogOut();
        }

        public async void CallNotebookAsync() {
            var collection = await Servces.ReadAsync(ChildName);
            var NotebookCollection = new List<Notebook>();
            foreach (var item in collection) {

                Notebook notebook = new Notebook {
                    UserID = item.Object.UserID,
                    Id = item.Key,
                    Name = item.Object.Name,
                    CreatedDate = item.Object.CreatedDate,
                    Color = item.Object.Color,
                    Desc = item.Object.Desc
                };
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
