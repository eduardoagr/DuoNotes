
using DuoNotes.Constants;
using DuoNotes.Fonts;
using DuoNotes.Model;
using DuoNotes.View;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    [AddINotifyPropertyChangedInterface]
    public class NotebooksPageModel {

        public Command SelectedItemCommand { get; set; }

        public Command ProfileCommnd { get; set; }

        public Command PageAppearCommand { get; set; }

        public Command<Notebook> DeleteNotebookCommand { get; set; }

        public Notebook SelectedNotebook { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebookNotes { get; set; }

        public Command<Frame> FabAnimationCommmand { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public FontImageSource Glyph { get; set; }

        public NotebooksPageModel() {

            FireBaseNotebookNotes = new ObservableCollection<NotebookNote>();

            PageAppearCommand = new Command(AppearAction);

            SelectedItemCommand = new Command(SelectedItemActionAsync);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            ProfileCommnd = new Command(NavigateCommandAsync);

            DeleteNotebookCommand = new Command<Notebook>(DeleteNotebookCommandAction);

            GetGlyph();

        }


        private void GetGlyph() {
            Glyph = new FontImageSource() {
                Glyph = IconFont.TrashCan,
                FontFamily = "fa",
                Size = 44
            };
        }

        public virtual async void AppearAction() {
            FireUser = await App.FirebaseService.GetProfileInformationAndRefreshTokenAsync();
            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
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

        public async virtual void SelectedItemActionAsync() {

            if (SelectedNotebook != null) {

                NotesPage notesPage = new NotesPage();
                Application.Current.Properties[AppConstant.SelectedNotebook] = SelectedNotebook;
                await Application.Current.MainPage.Navigation.PushAsync(notesPage, true);
                SelectedNotebook = null;
            }
        }

        private async void DeleteNotebookCommandAction(Notebook obj) {

            App.FirebaseService.DeleteNotebookNotAsync(obj.Id, AppConstant.Notebooks);

            var Notes = await App.FirebaseService.ReadAsync(AppConstant.Notes, obj.Id);
            foreach (var item in Notes) {
                App.FirebaseService.DeleteNotebookNotAsync(((Note)item).Id, AppConstant.Notes);
            }

            await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
        }
    }
}