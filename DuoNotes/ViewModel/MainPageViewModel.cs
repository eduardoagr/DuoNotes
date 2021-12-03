using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View.PopUps;

using Rg.Plugins.Popup.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class MainPageViewModel {



        public ICommand CreateNotebook { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public ICommand Logout { get; set; }

        public MainPageViewModel() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            App.services = new FirebaseServices(FireBaseNotebooks);

            Logout = new Command(LogOut);

            CreateNotebook = new Command(OpenCreateNewNotebookPopUp);

            App.services.ReadAsync("Notebooks");
        }

        private async void OpenCreateNewNotebookPopUp() {
            await PopupNavigation.Instance.PushAsync(new NotebookPopUp());
        }

        private void LogOut(object obj) {
            App.services.LogOut();
        }
    }

}
