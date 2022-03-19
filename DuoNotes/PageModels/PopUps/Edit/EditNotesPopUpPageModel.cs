using DuoNotes.Constants;
using DuoNotes.Model;


using Rg.Plugins.Popup.Services;

using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps.Edit {

    public class EditNotesPopUpPageModel : EditNotebookPopUpPageModel {

        public Note Note { get; set; }

        public string NotebookId { get; set; }

        public EditNotesPopUpPageModel() {

            UpdateCommand = new Command(UpdateAction);

            PageAppearCommand = new Command(PageAppearAction);

        }

        public override void PageAppearAction() {
            Note = Application.Current.Properties[AppConstant.EditNote] as Note;
            NotebookId = Application.Current.Properties[AppConstant.NotebookId] as string;
        }

        public override async void UpdateAction() {
            await App.FirebaseService.UpdateNoteAsync(Note.Id, Note.Name);
            await App.FirebaseService.ReadAsync(AppConstant.Notes, NotebookId);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
