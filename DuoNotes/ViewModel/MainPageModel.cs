
using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainPageModel {

        public ICommand SeletedItemCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public ICommand ProfileCommnd { get; set; }

        public ICommand PageAppearCommand { get; set; }

        public Notebook SelectedNotebook { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public Command<Frame> FabAnimationCommmand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public MainPageModel() {

            FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            App.FirebaseServices = new FirebaseServices(FireBaseNotebooks);

            PageAppearCommand = new Command(AppearAction);

            LogoutCommand = new Command(LogOutAction);

            SeletedItemCommand = new Command(SeletedItemActionAsync);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            ProfileCommnd = new Command(NavigateCommandAsync);
        }

        public MainPageModel(object o) {

            //FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            // AppConstant.services = new FirebaseServices(FireBaseNotebooks);
        }

        public virtual async void AppearAction() {
            FireUser = await App.FirebaseServices.GetProfileInformationAndRefreshToken();
            App.FirebaseServices.ReadAsync(AppConstant.Notebooks);
        }


        private async void NavigateCommandAsync() {
            await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
        }

        public virtual async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            await PopupNavigation.Instance.PushAsync(new NotebookPopUp());

        }

        public async virtual void SeletedItemActionAsync() {

            if (SelectedNotebook != null) {

                NotesPage notesPage = new NotesPage();
                await Application.Current.MainPage.Navigation.PushAsync(notesPage);
                var viewModel = notesPage.BindingContext as NotesPageModel;
                viewModel.NotebookAction(SelectedNotebook.Id);
                SelectedNotebook = null;
            }
        }

        private void LogOutAction() {
            App.FirebaseServices.LogOut();
        }
    }
}