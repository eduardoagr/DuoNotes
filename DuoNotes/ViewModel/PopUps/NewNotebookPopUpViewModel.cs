

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
    public class NewNotebookPopUpViewModel {

        public ICommand NewNotebookCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SelectedColorCommand { get; set; }


        readonly FirebaseServices Services;

        public List<Color> Colors { get; set; }

        public Color SelectedColor { get; set; }

        public Notebook Notebook { get; set; }

        public NewNotebookPopUpViewModel() {


            Services = App.services;

            Services.ReadAsync(App.Notebooks);

            Notebook = new Notebook {
                OnAnyPropertiesChanged = () => {

                    (NewNotebookCommand as Command).ChangeCanExecute();
                }
            };

            NewNotebookCommand = new Command(CreateNewNotebookAsync, CanCreateNotebook);

            CloseCommand = new Command(PerformCloseAction);

            Colors = ColorServices.GetColors();

            SelectedColorCommand = new Command(SelectColorAction);

        }

        public virtual async void PerformCloseAction() {
            await PopupNavigation.Instance.PopAsync();
        }

        private void SelectColorAction() {
            if (SelectedColor == null) {
                return;
            }

            Notebook.Color = SelectedColor.ToHex();
        }

        private async void CreateNewNotebookAsync() {
            if (Notebook == null) {
                return;
            }
            var id = Preferences.Get(App.UserID, string.Empty);

            Notebook = new Notebook {
                CreatedDate = DateTime.Now.ToString("D", new CultureInfo(App.languages)),
                UserID = id,
                Name = Notebook.Name,
                Desc = Notebook.Desc,
                Color = Notebook.Color,
            };



            await Services.InsertAsync(Notebook, App.Notebooks);
            await PopupNavigation.Instance.PopAsync();
            Services.ReadAsync(App.Notebooks);
        }


        private bool CanCreateNotebook() {

            return Notebook != null && !string.IsNullOrEmpty(Notebook.Name)
                && !string.IsNullOrEmpty(Notebook.Desc)
                && !string.IsNullOrEmpty(Notebook.Color);
        }
    }
}
