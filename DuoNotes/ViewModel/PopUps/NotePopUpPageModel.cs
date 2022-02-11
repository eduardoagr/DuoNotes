using DuoNotes.Model;
using DuoNotes.Services;

using Rg.Plugins.Popup.Services;

using System;
using System.Globalization;
using System.Windows.Input;

using Xamarin.Forms;

namespace DuoNotes.ViewModel.PopUps {

    public class NotePopUpPageModel : NotebookPopUpPageModel {

        public ICommand NewNoteCommand { get; set; }

        public Action<string> NotebookAction { get; set; }

        public string NotebookId { get; set; }

        public Note Note { get; set; }

        public NotePopUpPageModel() {

            Note = new Note {
                OnAnyPropertiesChanged = () => {

                    (NewNoteCommand as Command).ChangeCanExecute();
                }
            };

            NewNoteCommand = new Command(CreateNewNoteAsync, CanCreateNote);


            NotebookAction = (id) => {

                NotebookId = id;
            };

        }

        private async void CreateNewNoteAsync() {
            if (Note == null) {
                return;
            }

            Note = new Note() {
                CreatedDate = DateTime.Now.ToString("D", new CultureInfo(App.languages)),
                NotebookId = NotebookId,
                Name = Note.Name,
                Id = Note.Id,
                FileLocation = string.Empty
            };

            await App.services.InsertAsync(Note, App.Notes);
            await PopupNavigation.Instance.PopAsync();
        }


        private bool CanCreateNote() {

            return Note != null && !string.IsNullOrEmpty(Note.Name);
        }
    }
}