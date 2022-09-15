using DuoNotes.Constants;
using DuoNotes.Model;

using Rg.Plugins.Popup.Services;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps.Edit
{
    public class EditNotebookPopUpPageModel : InsertNotebookPopUpPageModel
    {

        public Command PageAppearCommand
        {
            get; set;
        }

        public Command UpdateCommand
        {
            get; set;
        }

        public new Notebook Notebook
        {
            get; set;
        }

        public Color CurrentColor
        {
            get; set;
        }

        public EditNotebookPopUpPageModel()
        {

            PageAppearCommand = new Command(PageAppearAction);

            UpdateCommand = new Command(UpdateAction);
        }

        public override void SelectColorAction()
        {
            if (SelectedColor != null)
            {
                Notebook.Color = CurrentColor.ToHex();
            }
        }

        public virtual async void UpdateAction()
        {
            await App.FirebaseService.UpdateNotebookAsync(Notebook.Id, Notebook.Color, Notebook.Name);
            await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
            await PopupNavigation.Instance.PopAsync();
        }



        public virtual void PageAppearAction()
        {

            Notebook = Application.Current.Properties[AppConstant.EditNotebook] as Notebook;

            CurrentColor = ColorConverters.FromHex(Notebook.Color);
        }
    }
}
