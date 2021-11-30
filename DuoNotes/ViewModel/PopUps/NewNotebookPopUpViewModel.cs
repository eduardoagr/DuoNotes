﻿

using DuoNotes.Model;
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

            CreateNotebook();
        }
        private void CreateNotebook() {
            Notebook = new Notebook {
                Name = Notebook.Name,
                Color = SelectedColor.ToHex(),
                CreatedDate = Notebook.CreatedDate,
                Desc = Notebook.Desc,
                UserID = App.UserID
            };
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
            await Services.InsertAsync(Notebook, "Notebook");
            Services.ReadAsync("Notebook");
        }

        private async void PerformClose() {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
