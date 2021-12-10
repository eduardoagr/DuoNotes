﻿using DuoNotes.Model;
using DuoNotes.Services;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using System;
using System.Globalization;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    [AddINotifyPropertyChangedInterface]
    public class NewNotePopUpViewModel : NewNotebookPopUpViewModel {

        readonly FirebaseServices Services;

        public Note Note { get; set; }

        public ICommand NewNoteCommand { get; set; }

        public NewNotePopUpViewModel() {


            Services = App.services;

            Note = new Note {
                OnAnyPropertiesChanged = () => {

                    (NewNoteCommand as Command).ChangeCanExecute();
                }
            };

            NewNoteCommand = new Command(CreateNewNoteAsync, CanCreateNote);

            CloseCommand = new Command(PerformCloseAction);
        }

        private async void PerformCloseAction() {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void CreateNewNoteAsync() {
            if (Note == null) {
                return;
            }

            Note.CreatedDate = DateTime.Now.ToString("D", new CultureInfo(App.languages));
            Note.Name = Note.Name;

            await Services.InsertAsync(Note, "Notes");
            await PopupNavigation.Instance.PopAsync();
            Services.ReadAsync("Notes");
        }


        private bool CanCreateNote() {

            return Note != null && !string.IsNullOrEmpty(Note.Name);
        }
    }
}