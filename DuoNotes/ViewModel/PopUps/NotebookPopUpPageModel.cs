

using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class NotebookPopUpPageModel {

        public ICommand NewNotebookCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SelectedColorCommand { get; set; }

        public ICommand DismissPopUpCommand { get; set; }

        public ICommand PageDisappearingCommand { get; set; }

        public List<Color> Colors { get; set; }

        public Color SelectedColor { get; set; }

        public Notebook Notebook { get; set; }

        public NotebookPopUpPageModel() {

            Notebook = new Notebook {
                OnAnyPropertiesChanged = () => {

                    (NewNotebookCommand as Command).ChangeCanExecute();
                }
            };

            NewNotebookCommand = new Command(CreateNewNotebookAsync, CanCreateNotebook);

            Colors = ColorServices.GetColors();


            SelectedColorCommand = new Command(SelectColorAction);


            DismissPopUpCommand = new Command(ClosePopUpAction);

        }

        private void ClosePopUpAction() {
            PopupNavigation.Instance.PopAsync();
        }

        private void SelectColorAction() {
            if (SelectedColor != null) {
                Notebook.Color = SelectedColor.ToHex();
            }
        }

        private async void CreateNewNotebookAsync() {
            if (Notebook != null) {

                Notebook = new Notebook {
                    CreatedDate = DateTime.Now.ToString("D", new CultureInfo(AppConstant.languages)),
                    UserID = Preferences.Get(AppConstant.UserID, string.Empty),
                    Name = Notebook.Name,
                    Desc = Notebook.Desc,
                    Color = Notebook.Color,
                };
            }

            await AppConstant.services.InsertAsync(Notebook, AppConstant.Notebooks);
            await PopupNavigation.Instance.PopAsync();
            AppConstant.services.ReadAsync(AppConstant.Notebooks);
        }


        private bool CanCreateNotebook() {

            return Notebook != null && !string.IsNullOrEmpty(Notebook.Name)
                && !string.IsNullOrEmpty(Notebook.Desc)
                && !string.IsNullOrEmpty(Notebook.Color);
        }
    }
}
