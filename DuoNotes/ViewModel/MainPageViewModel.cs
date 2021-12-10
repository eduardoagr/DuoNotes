
using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel {

        public NotebookNote SelectedNotebookNote { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public ICommand SeletedItemCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public Command<Frame> FabAnimationCommmand { get; set; }

        public ICommand ProfileCommand { get; set; }

        public MainPageViewModel() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            App.services = new FirebaseServices(FireBaseNotebooks);

            LogoutCommand = new Command(LogOutAction);

            SeletedItemCommand = new Command(SeletedItemActionAsync);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            App.services.ReadAsync("Notebooks");
        }

        private async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Wait a moment
            await Task.Delay(100);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            await PopupNavigation.Instance.PushAsync(new NotebookPopUp());

        }

        private async void SeletedItemActionAsync() {
            if (SelectedNotebookNote == null) {
                return;
            }

            await App.Current.MainPage.Navigation.PushAsync(new NotesPage());
            SelectedNotebookNote = null;
        }

        private void LogOutAction() {
            App.services.LogOut();
        }
    }

}
