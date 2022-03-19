

using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.Generic;
using System.Globalization;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class InsertNotebookPopUpPageModel {

        public Command NewNotebookCommand { get; set; }

        public Command CloseCommand { get; set; }

        public Command SelectedColorCommand { get; set; }

        public Command DismissPopUpCommand { get; set; }

        public List<Color> Colors { get; set; }

        public Color SelectedColor { get; set; }

        public Notebook Notebook { get; set; }

        public InsertNotebookPopUpPageModel() {

            Notebook = new Notebook {
                OnAnyPropertiesChanged = () => {

                    NewNotebookCommand.ChangeCanExecute();
                }
            };

            NewNotebookCommand = new Command(CreateNewNotebookAsync, CanCreateNotebook);

            Colors = ColorService.GetColors();

            SelectedColorCommand = new Command(SelectColorAction);

            DismissPopUpCommand = new Command(ClosePopUpAction);

        }

        public virtual void ClosePopUpAction() {
            PopupNavigation.Instance.PopAsync();
        }

        public virtual void SelectColorAction() {
            if (SelectedColor != null) {
                Notebook.Color = SelectedColor.ToHex();
            }
        }

        private async void CreateNewNotebookAsync() {
            if (Notebook != null) {

                Notebook = new Notebook {
                    CreatedDate = DateTime.Now.ToString("D", new CultureInfo(AppConstant.languages)),
                    Name = Notebook.Name,
                    Color = Notebook.Color,
                    UserID = Preferences.Get(AppConstant.UserID, string.Empty),
                };
            }

            await App.FirebaseService.InsertAsync(Notebook, AppConstant.Notebooks);
            await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
            await PopupNavigation.Instance.PopAsync();

        }


        private bool CanCreateNotebook() {

            return Notebook != null && !string.IsNullOrEmpty(Notebook.Name)
                && !string.IsNullOrEmpty(Notebook.Color);
        }
    }
}
