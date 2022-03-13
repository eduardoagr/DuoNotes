using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.PageModels.PopUps;
using DuoNotes.Pages;
using DuoNotes.View.PopUps;

using Rg.Plugins.Popup.Services;

using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class NotesPageModel : NotebooksPageModel {

        public Notebook Notebook { get; set; }

        public Note SelectedNote { get; set; }

        public Command<Note> DeleteNoteCommand { get; set; }

        public Command PageDisappearCommand { get; set; }

        public NotesPageModel() {

            DeleteNoteCommand = new Command<Note>(DeleteNoteAction);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            PageDisappearCommand = new Command(PageDisappearAction);
        }

        public override async void AppearAction() {

            Notebook = Application.Current.Properties[AppConstant.SelectedNotebook] as Notebook;

            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notes, Notebook.Id);

        }

        public override async void TextToSearchAction(string SeachTerm) {

            if (!string.IsNullOrEmpty(SeachTerm)) {

                var FilteredItems = FireBaseNotebookNotes.Where(item =>
                item.Name.ToLowerInvariant().Contains(SeachTerm.ToLowerInvariant())).ToList();

                FireBaseNotebookNotes.Clear();

                foreach (var Item in FilteredItems) {
                    FireBaseNotebookNotes.Add(Item);
                }

            } else {
                FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notes, Notebook.Id);
                IsVisible = false;
            }
        }

        public override async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            var notesPopUp = new NotesPopUp();
            await PopupNavigation.Instance.PushAsync(notesPopUp, true);
            var viewModel = notesPopUp.BindingContext as NotePopUpPageModel;
            viewModel.NotebookAction(Notebook.Id);
        }

        public override async void SelectedItemActionAsync(NotebookNote notebookNote) {

            if (notebookNote is NotebookNote notebook) {

                var edit = new EditorPage();
                Application.Current.Properties[AppConstant.SelectedNote] = notebook;
                await Application.Current.MainPage.Navigation.PushAsync(edit);
            }
        }


        private async void DeleteNoteAction(Note obj) {

            string ext = ".html";

            App.FirebaseService.DeleteNotebookNotAsync(obj.Id, AppConstant.Notes);

            App.AzureService.DeleteFileFromBlobStorage($"{obj.Name}{ext}");

            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notes, Notebook.Id);
        }

        public virtual void PageDisappearAction() {
            FireBaseNotebookNotes.Clear();
        }
    }
}
