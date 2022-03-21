using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.PageModels.PopUps;
using DuoNotes.Pages;
using DuoNotes.Pages.PopUps.Edit;
using DuoNotes.View.PopUps;

using Rg.Plugins.Popup.Services;

using System.Linq;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class NotesPageModel : NotebooksPageModel {

        public Notebook Notebook { get; set; }

        public Command PageDisappearCommand { get; set; }

        public bool TitleVisibility { get; set; }

        public bool SearchBtonVisibility { get; set; }

        public NotesPageModel() {

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            PageDisappearCommand = new Command(PageDisappearAction);

            TitleVisibility = true;

            SearchBtonVisibility = true;
        }

        public override void SearchPressAction() {

            TitleVisibility = true;
            SearchBtonVisibility = false;
        }

        public override async void AppearAction() {

            Notebook = Application.Current.Properties[AppConstant.SelectedNotebook] as Notebook;

            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notes, Notebook.Id);

        }

        public override void SwitchVisibilityAction() {

            if (TitleVisibility) {
                SearchBtonVisibility = false;
                SearchBarVisibility = true;
                TitleVisibility = false;
            } else {
                SearchBtonVisibility = true;
                SearchBarVisibility = false;
                TitleVisibility = true;
            }
        }

        public override async void TextToSearchAction(string SeachTerm) {

            if (!string.IsNullOrEmpty(SeachTerm)) {

                FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notes,
                    Notebook.Id);

                var FilteredItems = FireBaseNotebookNotes.Where(item =>
                item.Name.ToLowerInvariant().Contains(SeachTerm.ToLowerInvariant())).ToList();

                FireBaseNotebookNotes.Clear();

                foreach (var Item in FilteredItems) {
                    FireBaseNotebookNotes.Add(Item);
                }

            } else {
                SearchBarVisibility = false;
                TitleVisibility = true;
                SearchBtonVisibility = true;
            }
        }

        public override async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            var notesPopUp = new InsertNotesPopUpPage();
            await PopupNavigation.Instance.PushAsync(notesPopUp, true);
            var viewModel = notesPopUp.BindingContext as InsertNotePopUpPageModel;
            viewModel.NotebookAction(Notebook.Id);
        }

        public override async void SelectedItemActionAsync() {

            if (SelectedItem != null) {

                var edit = new EditorPage();
                Application.Current.Properties[AppConstant.SelectedNote] = SelectedItem;
                await Application.Current.MainPage.Navigation.PushAsync(edit);
            }
        }

        public override async void DeleteNotebookCommandAction(NotebookNote obj) {

            var newObj = obj as Note;

            string ext = ".html";

            App.FirebaseService.DeleteNotebookNotAsync(newObj.Id, AppConstant.Notes);

            App.AzureService.DeleteFileFromBlobStorage($"{obj.Name}{ext}");

            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notes, Notebook.Id);

        }

        public override async void EditNotebookNoteAction(NotebookNote obj) {

            var note = obj as Note;

            Application.Current.Properties[AppConstant.EditNote] = note;
            Application.Current.Properties[AppConstant.NotebookId] = Notebook.Id;

            await PopupNavigation.Instance.PushAsync(new EditNotesPopUpPage());

        }
        public virtual void PageDisappearAction() {
            FireBaseNotebookNotes.Clear();
        }
    }
}
