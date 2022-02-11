using DuoNotes.Model;
using DuoNotes.View.PopUps;
using DuoNotes.ViewModel.PopUps;

using Rg.Plugins.Popup.Services;

using System;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    public class NotesPageModel : MainPageModel {

        public Action<string> NotebookAction { get; set; }

        public string NotebookId { get; set; }

        public Note SeletedNote { get; set; }

        public NotesPageModel() {

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            NotebookAction = (id) => {

                NotebookId = id;
            };

        }


        public override async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            var notesPopUp = new NotesPopUp();
            await PopupNavigation.Instance.PushAsync(notesPopUp);
            var viewModel = notesPopUp.BindingContext as NotePopUpPageModel;
            viewModel.NotebookAction(NotebookId);
            SelectedNotebook = null;
        }

        public override async void SeletedItemActionAsync() {
            base.SeletedItemActionAsync();

            await Application.Current.MainPage.DisplayAlert(String.Empty, "I have to do my own implementation", "OK");

        }
    }
}
