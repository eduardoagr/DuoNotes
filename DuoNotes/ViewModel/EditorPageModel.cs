using DuoNotes.Model;

using System;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    public class EditorPageModel {

        public Action<Note> NoteAction { get; set; }

        public Note Note { get; set; }

        public Command PageDisappearCommand { get; set; }

        public Command SaveCommand { get; set; }

        public EditorPageModel() {

            PageDisappearCommand = new Command(PageDisappearAction);

            SaveCommand = new Command(SveAction);


            NoteAction = (note) => {

                Note = Note;

            };

        }

        private void PageDisappearAction() {


        }

        private void SveAction() {
            throw new NotImplementedException();
        }

    }
}
