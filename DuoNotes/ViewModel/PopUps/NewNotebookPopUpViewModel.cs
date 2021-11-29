

using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class NewNotebookPopUpViewModel {

        FirebaseServices Services;

        public List<Color> Colors { get; set; }

        public Color SelectedColor { get; set; }

        public ICommand NewNotebook { get; set; }

        public ICommand Close { get; set; }

        public ICommand SelectedColorCommand { get; set; }

        public NewNotebookPopUpViewModel() {

            Services = new FirebaseServices();

            NewNotebook = new Command(CreateNewNotebook);

            Close = new Command(PerformClose);

            Colors = ColorServices.GetItems();

            SelectedColorCommand = new Command(SelectColorAction);
        }

        private void SelectColorAction() {
            if (SelectedColor == null) {
                return;
            }
        }

        private void CreateNewNotebook() {
            throw new NotImplementedException();
        }

        private async void PerformClose() {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
