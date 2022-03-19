
using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Pages.PopUps.Edit;
using DuoNotes.View;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    [AddINotifyPropertyChangedInterface]
    public class NotebooksPageModel {

        public Command ProfileCommnd { get; set; }

        public Command PageAppearCommand { get; set; }

        public Command SelectedItemLongPressCommand { get; set; }

        public Command SwitchVisibilityCommand { get; set; }

        public Command SelectedItemCommand { get; set; }

        public Command SearchPressCommand { get; set; }

        public Command<string> TextToSearchCommand { get; set; }

        public Command<NotebookNote> DeleteNotebookNoteCommand { get; set; }

        public Command<NotebookNote> EditNotebookNoteCommand { get; set; }

        public Command<Frame> FabAnimationCommmand { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebookNotes { get; set; }

        public ObservableCollection<NotebookNote> NewFireBaseNotebookNotes { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public FontImageSource Glyph { get; set; }

        public NotebookNote SelectedItem { get; set; }

        public bool SearchBarVisibility { get; set; }

        public bool ProfileVisibility { get; set; }

        public NotebooksPageModel() {

            FireBaseNotebookNotes = new ObservableCollection<NotebookNote>();

            NewFireBaseNotebookNotes = new ObservableCollection<NotebookNote>();

            PageAppearCommand = new Command(AppearAction);

            SelectedItemCommand = new Command(SelectedItemActionAsync);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            ProfileCommnd = new Command(NavigateCommandAsync);

            DeleteNotebookNoteCommand = new Command<NotebookNote>(DeleteNotebookCommandAction);

            SwitchVisibilityCommand = new Command(SwitchVisibilityAction);

            TextToSearchCommand = new Command<string>(TextToSearchAction);

            SearchPressCommand = new Command(SearchPressAction);

            EditNotebookNoteCommand = new Command<NotebookNote>(EditNotebookNoteAction);

            ProfileVisibility = true;
        }

        public virtual void SearchPressAction() {

            SearchBarVisibility = false;
            ProfileVisibility = true;
        }

        public virtual async void AppearAction() {
            FireUser = await App.FirebaseService.GetProfileInformationAndRefreshTokenAsync();
            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);

        }

        public virtual async void TextToSearchAction(string SeachTerm) {

            if (!string.IsNullOrWhiteSpace(SeachTerm)) {

                FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);

                var FilteredItems = FireBaseNotebookNotes.Where(item =>
                item.Name.ToLowerInvariant().Contains(SeachTerm.ToLowerInvariant())).ToList();

                FireBaseNotebookNotes.Clear();

                foreach (var Item in FilteredItems) {
                    FireBaseNotebookNotes.Add(Item);
                }

            } else {
                FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
                SearchBarVisibility = false;
                ProfileVisibility = true;
            }
        }

        public virtual void SwitchVisibilityAction() {

            SearchBarVisibility = !SearchBarVisibility;

            if (SearchBarVisibility) {
                ProfileVisibility = false;
            } else {
                ProfileVisibility = true;
            }
        }

        private async void NavigateCommandAsync() {
            await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
        }

        public virtual async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            await PopupNavigation.Instance.PushAsync(new InsertNotebookPopUpPage());

        }

        public async virtual void SelectedItemActionAsync() {

            if (SelectedItem != null) {

                NotesPage notesPage = new NotesPage();
                Application.Current.Properties[AppConstant.SelectedNotebook] = SelectedItem;
                await Application.Current.MainPage.Navigation.PushAsync(notesPage, true);
            }
        }

        public virtual async void DeleteNotebookCommandAction(NotebookNote obj) {

            var newObj = obj as Notebook;
            string ext = ".html";

            App.FirebaseService.DeleteNotebookNotAsync(newObj.Id, AppConstant.Notebooks);

            //We want to read, but we do not want to update
            var Notes = await App.FirebaseService.ReadWithOutUpdateAsync(AppConstant.Notes, newObj.Id);
            foreach (var item in Notes) {
                var note = item as Note;
                App.FirebaseService.DeleteNotebookNotAsync(note.Id, AppConstant.Notes);
                await App.AzureService.DeleteFileFromBlobStorage($"{note.Name}{ext}");
            }

            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
        }

        public virtual async void EditNotebookNoteAction(NotebookNote obj) {

            var notebook = obj as Notebook;

            Application.Current.Properties[AppConstant.EditNotebook] = notebook;

            await PopupNavigation.Instance.PushAsync(new EditNotebookPopUpPage());

        }

    }
}