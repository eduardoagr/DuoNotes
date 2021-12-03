

using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class NewNotebookPopUpViewModel {

        FirebaseServices Services;

        public List<Color> Colors { get; set; }

        public Color SelectedColor { get; set; }

        //public ObservableCollection<NotebookNote> FireBaseNotebooks { get; set; }

        public Notebook Notebook { get; set; }

        public ICommand NewNotebook { get; set; }

        public ICommand Close { get; set; }

        public ICommand SelectedColorCommand { get; set; }

        public NewNotebookPopUpViewModel() {

            //FireBaseNotebooks = new ObservableCollection<NotebookNote>();

            Services = App.services;//new FirebaseServices();

            Notebook = new Notebook {
                OnAnyPropertiesChanged = () => {

                    (NewNotebook as Command).ChangeCanExecute();
                }
            };

            NewNotebook = new Command(CreateNewNotebook, CanCreateNotebook);

            Close = new Command(PerformClose);

            Colors = ColorServices.GetItems();

            SelectedColorCommand = new Command(SelectColorAction);

        }

        private async void PerformClose() {
            await PopupNavigation.Instance.PopAsync();
        }

        private void SelectColorAction() {
            if (SelectedColor == null) {
                return;
            }

            Notebook.Color = SelectedColor.ToHex();
        }

        private async void CreateNewNotebook() {
            if (Notebook == null) {
                return;
            }

            Notebook.CreatedDate = DateTime.Now.ToString("D", new CultureInfo(App.languages));
            Notebook.UserID = App.UserID;
            Notebook.Name = Notebook.Name;
            Notebook.Desc = Notebook.Desc;

            await Services.InsertAsync(Notebook, "Notebooks");
            await PopupNavigation.Instance.PopAsync();
            Services.ReadAsync("Notebooks");
        }


        private bool CanCreateNotebook() {

            return Notebook != null && !string.IsNullOrEmpty(Notebook.Name)
                && !string.IsNullOrEmpty(Notebook.Desc)
                && !string.IsNullOrEmpty(Notebook.Color);
        }
    }
}
