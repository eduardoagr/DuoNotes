using DuoNotes.Constants;
using DuoNotes.Model;

using Rg.Plugins.Popup.Services;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace DuoNotes.PageModels.PopUps {

    public class NotePopUpPageModel : NotebookPopUpPageModel {

        public Command NewNoteCommand { get; set; }

        public Action<string> NotebookAction { get; set; }

        public string NotebookId { get; set; }

        public Note Note { get; set; }

        public NotePopUpPageModel() {

            Note = new Note {
                OnAnyPropertiesChanged = () => {

                    NewNoteCommand.ChangeCanExecute();
                }
            };

            NewNoteCommand = new Command(CreateNewNoteAsync, CanCreateNote);


            NotebookAction = (id) => {

                NotebookId = id;
            };

        }

        private async void CreateNewNoteAsync() {
            if (Note != null) {

                Note = new Note() {
                    CreatedDate = DateTime.Now.ToString("D", new CultureInfo(AppConstant.languages)),
                    Name = Note.Name,
                    FileLocation = string.Empty,
                    Id = Note.Id,
                    NotebookId = NotebookId,  
                };
            }

            await App.FirebaseService.InsertAsync(Note, AppConstant.Notes);
            await App.FirebaseService.ReadAsync(AppConstant.Notes, NotebookId);
            await PopupNavigation.Instance.PopAsync();
        }


        private bool CanCreateNote() {

            return Note != null && !string.IsNullOrEmpty(Note.Name);
        }
    }
}