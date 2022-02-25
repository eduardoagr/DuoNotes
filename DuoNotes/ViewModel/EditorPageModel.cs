using System;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    public class EditorPageModel {

        public Action<string> NoteAction { get; set; }

        public string NoteId { get; set; }

        public Command SaveCommand { get; set; }

        public EditorPageModel() {

            NoteAction = (id) => {

                NoteId = id;
            };

            SaveCommand = new Command(SveAction);
        }

        private void SveAction() {
            throw new NotImplementedException();
        }
    }
}
