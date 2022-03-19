using DuoNotes.Constants;
using DuoNotes.Model;

using Rg.Plugins.Popup.Services;

using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps.Edit {
    public class EditNotebookPopUpPageModel : InsertNotebookPopUpPageModel {

        public Command PageAppearCommand { get; set; }

        public Command UpdateCommand { get; set; }

        public new Notebook Notebook { get; set; }

        public EditNotebookPopUpPageModel() {

            PageAppearCommand = new Command(PageAppearAction);

            UpdateCommand = new Command(UpdateAction);
        }

        public virtual async void UpdateAction() {
            var color = SelectedColor.ToHex();
            await App.FirebaseService.UpdateNotebookAsync(Notebook.Id, color, Notebook.Name);
            await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
            await PopupNavigation.Instance.PopAsync();
        }

        public virtual void PageAppearAction() {

            Notebook = Application.Current.Properties[AppConstant.EditNotebook] as Notebook;
        }
    }
}
