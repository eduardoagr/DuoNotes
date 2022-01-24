
using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel {

        public ICommand SeletedItemCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public ICommand ProfileCommnd { get; set; }

        public ICommand PageAppearCommand { get; set; }

        public Notebook SelectedNotebook { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public Command<Frame> FabAnimationCommmand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public string DisplayName { get; set; }

        public MainPageViewModel() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            App.services = new FirebaseServices(FireBaseNotebooks);

            PageAppearCommand = new Command(AppearAction);

            LogoutCommand = new Command(LogOutAction);

            SeletedItemCommand = new Command(SeletedItemActionAsync);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            ProfileCommnd = new Command(NavigateCommandAsync);

            App.services.ReadAsync(App.Notebooks);


        }

        private async void AppearAction() {
            FireUser = await App.services.GetProfileInformationAndRefreshToken();
        }

        private async void NavigateCommandAsync() {
            await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
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

            if (SelectedNotebook == null) {
                return;
            }

            NotesPage notesPage = new NotesPage();
            await App.Current.MainPage.Navigation.PushAsync(notesPage);
            var viewModel = notesPage.BindingContext as NotesPageViewModel;
            viewModel.RecivedSelectedNotebookAccion(SelectedNotebook);
            SelectedNotebook = null;

        }

        private void LogOutAction() {
            App.services.LogOut();
        }
    }
}