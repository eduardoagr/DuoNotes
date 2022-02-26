using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Pages;
using DuoNotes.View.PopUps;
using DuoNotes.ViewModel.PopUps;

using Rg.Plugins.Popup.Services;

using System;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    public class NotesPageModel : NotebooksPageModel {

        public Action<string> NotebookAction { get; set; }

        public string NotebookId { get; set; }

        public Note SeletedNote { get; set; }

        public Command<Note> DeleteNoteCommand { get; set; }

        public Command PageDisappearCommand { get; set; }

        public NotesPageModel() {

            DeleteNoteCommand = new Command<Note>(DeleteNoteAction);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            PageDisappearCommand = new Command(PageDisappearAction);

            NotebookAction = (id) => {

                NotebookId = id;

            };
        }



        public override async void AppearAction() {
            base.AppearAction();

            FireBaseNotebooks = await App.FirebaseServices.ReadAsync(AppConstant.Notes, NotebookId);
        }

        private void PageDisappearAction() {
            FireBaseNotebooks.Clear();
        }

        public override async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            var notesPopUp = new NotesPopUp();
            await PopupNavigation.Instance.PushAsync(notesPopUp, true);
            var viewModel = notesPopUp.BindingContext as NotePopUpPageModel;
            viewModel.NotebookAction(NotebookId);
        }

        public override async void SeletedItemActionAsync() {
            base.SeletedItemActionAsync();

            var EditPage = new EditorPage();

            await Application.Current.MainPage.Navigation.PushAsync(EditPage);

            var viewModel = EditPage.BindingContext as EditorPageModel;

            viewModel.NoteAction(SeletedNote);
        }

        private async void DeleteNoteAction(Note obj) {

            App.FirebaseServices.DeleteNotebookNotAsync(obj.Id, AppConstant.Notes);

            FireBaseNotebooks = await App.FirebaseServices.ReadAsync(AppConstant.Notes, NotebookId);
        }
    }
}
