using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View.PopUps;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class MainPageViewModel {

        public NotebookNote SelectedNotebookNote { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public ICommand CreateNotebook { get; set; }

        public ICommand SeletedItemCommand { get; set; }

        public ICommand Logout { get; set; }

        public MainPageViewModel() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            CreateNotebook = new Command(OpenCreateNewNotebookPopUp);

            App.services = new FirebaseServices(FireBaseNotebooks);

            Logout = new Command(LogOut);

            SeletedItemCommand = new Command(SeletedItemAction);

            App.services.ReadAsync("Notebooks");
        }

        private void SeletedItemAction() {
            if (SelectedNotebookNote == null) {
                return;
            }

            //Todo: Navigte to new page and mke this null
        }

        private async void OpenCreateNewNotebookPopUp() {
            await PopupNavigation.Instance.PushAsync(new NotebookPopUp());
        }

        private void LogOut(object obj) {
            App.services.LogOut();
        }
    }

}
