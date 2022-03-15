using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Resources;

using System.IO;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class EditorPageModel : NotesPageModel {

        public Note Note { get; set; }

        public Command SaveCommand { get; set; }

        public Command ShareCommand { get; set; }

        public string HtmlText { get; set; }

        public EditorPageModel() {

            SaveCommand = new Command(SaveAction);
        }

        public override async void AppearAction() {

            Note = Application.Current.Properties[AppConstant.SelectedNote] as Note;

            var NewNote = await App.FirebaseService.ReadByIdAsync(AppConstant.Notes, Note.Id);

            if (!string.IsNullOrEmpty(NewNote.FileLocation)) {
                var ext = ".html";
                HtmlText = await App.AzureService.GetBlobStorage($"{NewNote.Name}{ext}");
            }
        }

        private async void SaveAction() {

            if (HtmlText != "<p><br></p>" || !string.IsNullOrEmpty(HtmlText)) {

                var AppDirctory = FileSystem.CacheDirectory;

                var filePath = Path.Combine(AppDirctory, $"{Note.Name}.html");

                var FileName = Path.GetFileName(filePath);

                using (var sw = new StreamWriter(File.Create(filePath))) {

                    sw.WriteLine(HtmlText);
                }

                var location = await App.AzureService.UploadToAzureBlobStorage(filePath, FileName);

                Note.FileLocation = location;

                App.FirebaseService.UpdateNoteFileLocationAsync(Note.Id, Note);

                File.Delete(filePath);
            } else {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.EditorError, AppResources.OK);
            }
        }
    }
}
