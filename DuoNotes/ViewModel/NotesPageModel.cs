using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View.PopUps;
using DuoNotes.ViewModel.PopUps;

using Rg.Plugins.Popup.Services;

using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    public class NotesPageModel : MainPageModel {


        readonly FirebaseServices Services;

        public string RecivedSelectedNotebookID { get; set; }

        public Note SeletedNote { get; set; }

        public NotesPageModel() {

            Services = App.services;

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            MessagingCenter.Subscribe<MainPageModel, string>(this, App.NotebookID, (sender, val) => {

                RecivedSelectedNotebookID = val;

                MessagingCenter.Unsubscribe<MainPageModel, string>(this, App.NotebookID);

                Services.ReadAsync(App.Notes, RecivedSelectedNotebookID);
            });


        }


        private async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            var notesPopUp = new NotesPopUp();
            await PopupNavigation.Instance.PushAsync(notesPopUp);
            var viewModel = notesPopUp.BindingContext as NotePopUpPageModel;
            viewModel.NotebookkIdAction(RecivedSelectedNotebookID);
            SelectedNotebook = null;
        }

        public override async void SeletedItemActionAsync() {
            base.SeletedItemActionAsync();

            await Application.Current.MainPage.DisplayAlert(String.Empty, "I have to do my own implementation", "OK");

        }
    }
}
