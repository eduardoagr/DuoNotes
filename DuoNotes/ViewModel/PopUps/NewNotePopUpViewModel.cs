using DuoNotes.Model;
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

        public ICommand NewNoteCommand { get; set; }

        public string RecivedSelectedNotebookID { get; set; }

        public Note Note { get; set; }

        public NewNotePopUpViewModel() {

            Services = App.services;

            Note = new Note {
                OnAnyPropertiesChanged = () => {

                    (NewNoteCommand as Command).ChangeCanExecute();
                }
            };

            NewNoteCommand = new Command(CreateNewNoteAsync, CanCreateNote);

            Services.ReadAsync(App.Notes, RecivedSelectedNotebookID);
        }

        public override void PerformCloseAction() {
            base.PerformCloseAction();

            PopupNavigation.Instance.PopAsync();

            Services.ReadAsync(App.Notes, RecivedSelectedNotebookID);
        }
        private async void CreateNewNoteAsync() {
            if (Note == null) {
                return;
            }

            Note.CreatedDate = DateTime.Now.ToString("D", new CultureInfo(App.languages));
            Note.NotebookId = RecivedSelectedNotebookID;
            Note.Name = Note.Name;
            Note.Id = Note.Id;
            Note.FileLocation = string.Empty;

            await Services.InsertAsync(Note, App.Notes);
            await PopupNavigation.Instance.PopAsync();

            Services.ReadAsync(App.Notes, RecivedSelectedNotebookID);
        }


        private bool CanCreateNote() {

            return Note != null && !string.IsNullOrEmpty(Note.Name);
        }
    }
}