using DuoNotes.Constants;
using DuoNotes.Model;

using Rg.Plugins.Popup.Services;

using System;
using System.Globalization;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps.Edit {
    internal class EditNotebookPopUpPageModel : InsertNotebookPopUpPageModel {

        public Command PageAppearCommand { get; set; }

        public Command UpdateCommand { get; set; }

        public new Notebook Notebook { get; set; }

        public EditNotebookPopUpPageModel() {

            PageAppearCommand = new Command(PageAppearAction);

            UpdateCommand = new Command(UpdateAction);
        }

        private async void UpdateAction() {

            if (Notebook != null) {

                Notebook = new Notebook {
                    CreatedDate = DateTime.Now.ToString("D", new CultureInfo(AppConstant.languages)),
                    Name = Notebook.Name,
                    Color = Notebook.Color,
                    UserID = Preferences.Get(AppConstant.UserID, string.Empty),
                };
            }

            App.FirebaseService.UpdateNoteBookTitleAsync(Notebook.Id, Notebook.Name);
            await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
            await PopupNavigation.Instance.PopAsync();
        }

        private void PageAppearAction() {

            Notebook = Application.Current.Properties[AppConstant.SelectedNotebook] as Notebook;
        }
    }
}
