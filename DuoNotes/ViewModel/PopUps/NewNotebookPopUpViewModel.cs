

using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class NewNotebookPopUpViewModel {

        FirebaseServices Services;

        public List<Color> Colors { get; set; }

        public Color SelectedColor { get; set; }

        public Notebook Notebook { get; set; }

        public ICommand NewNotebook { get; set; }

        public ICommand Close { get; set; }

        public ICommand SelectedColorCommand { get; set; }

        public NewNotebookPopUpViewModel() {

            Services = new FirebaseServices();

            NewNotebook = new Command(CreateNewNotebook);

            Close = new Command(PerformClose);

            Colors = ColorServices.GetItems();

            SelectedColorCommand = new Command(SelectColorAction);

            Notebook = new Notebook();

        }

        private void SelectColorAction() {
            if (SelectedColor == null) {
                return;
            }
        }

        private async void CreateNewNotebook() {
            if (Notebook == null) {
                return;
            }
            DateTime dt = new DateTime();
            var result = dt.ToString("D", new CultureInfo(App.languages));
            Notebook.CreatedDate = result;
            Notebook.UserID = App.UserID;
            Notebook.Name = Notebook.Name;
            Notebook.Desc = Notebook.Desc;
            Notebook.Color = SelectedColor.ToHex();
            await Services.InsertAsync(Notebook, "Notebook");
            Services.ReadAsync("Notebook");
        }

        private async void PerformClose() {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
