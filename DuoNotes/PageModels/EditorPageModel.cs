using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Resources;

using System.Diagnostics;
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

            if (string.IsNullOrEmpty(NewNote.FileLocation)) {
                await App.Current.MainPage.DisplayAlert("error", "Nothing", "OK");
            }
        }

        private async void SaveAction() {

            if (HtmlText == "<p><br></p>" || string.IsNullOrEmpty(HtmlText)) {

                string action = await Application.Current.MainPage.DisplayActionSheet
                    (AppResources.EditorError, AppResources.Cancel,
                    null, AppResources.ContinueEditing, AppResources.GoBack);

                if (action == AppResources.GoBack) {
                    await Application.Current.MainPage.Navigation.PopAsync();
                }

            } else {
                var AppDirctory = FileSystem.CacheDirectory;

                var filePath = Path.Combine(AppDirctory, $"{Note.Name}.html");

                var FileName = Path.GetFileName(filePath);

                using (var sw = new StreamWriter(File.Create(filePath))) {

                    sw.WriteLine(HtmlText);
                }

                var location = await App.AzureService.UploadToAzureBlobStorage(filePath, FileName);

                App.FirebaseService.UpdateNoteFileLocationAsync(Note.Id, location);

                File.Delete(filePath);
            }
        }
    }
}
