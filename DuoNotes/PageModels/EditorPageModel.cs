using DuoNotes.Constants;
using DuoNotes.Model;

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

            await App.FirebaseService.ReadByIdAsync(AppConstant.Notes, Note.Id);


            if (string.IsNullOrEmpty(Note.FileLocation)) {
                await App.Current.MainPage.DisplayAlert("error", "Nothing", "OK");
            }
        }

        private async void SaveAction() {

            var AppDirctory = FileSystem.CacheDirectory;

            var filePath = Path.Combine(AppDirctory, $"{Note.Name}.html");

            var FileName = Path.GetFileName(filePath);

            using (var sw = new StreamWriter(File.Create(filePath))) {

                sw.WriteLine(HtmlText);
            }

            var location = await App.AzureService.UploadToAzureBlobStorage(filePath, FileName);

            App.FirebaseService.UpdateNote(Note.Id, location);

            File.Delete(filePath);

        }

        public override void PageDisappearAction() {


        }
    }
}
    