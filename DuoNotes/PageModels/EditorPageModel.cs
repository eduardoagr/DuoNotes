
using Acr.UserDialogs;

using DuoNotes.Constants;
using DuoNotes.Fonts;
using DuoNotes.Model;
using DuoNotes.Pages.PopUps;
using DuoNotes.Utils;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

using Rg.Plugins.Popup.Services;

using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.RichTextEditor;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class EditorPageModel : NotesPageModel {

        public Note Note { get; set; }

        public Command OcrCommand { get; set; }

        public ObservableCollection<object> ToolbarOptionsCollection { get; set; }

        public ComputerVisionClient VisionClient { get; set; }

        public string HtmlText { get; set; }

        public string PlainText { get; set; }

        public string PhotoPath { get; set; }

        public string PhotoName { get; set; }

        public EditorPageModel() {

            ToolbarOptionsCollection = new ObservableCollection<object>();

            VisionClient = ComputerVision.Authenticate(AppConstant.ComputerVisionEndPoint, AppConstant.ComputerVisionKey);

            AddToolbarItems();
        }

        public void AddToolbarItems() {

            //Insert a new item to the custom toolbar collection.
            var ocrBton = new SfButton {
                Style = (Style)Application.Current.Resources[AppConstant.EdtorToolBarBtonsstyle],
                Text = MaterialIcons.MagnifyExpand,
                Command = new Command(OcrAction)
            };
            var saveButon = new SfButton {
                Text = MaterialIcons.ContentSave,
                Style = (Style)Application.Current.Resources[AppConstant.EdtorToolBarBtonsstyle],
                Command = new Command(SaveAction)
            };
            var shareButon = new SfButton {
                Text = MaterialIcons.ShareVariant,
                Style = (Style)Application.Current.Resources[AppConstant.EdtorToolBarBtonsstyle],
                Command = new Command(ShareAction)
            };

            ToolbarOptionsCollection.Add(ToolbarOptions.Bold);
            ToolbarOptionsCollection.Add(ToolbarOptions.Italic);
            ToolbarOptionsCollection.Add(ToolbarOptions.Underline);
            ToolbarOptionsCollection.Add(ToolbarOptions.FontColor);
            ToolbarOptionsCollection.Add(ToolbarOptions.FontSize);
            ToolbarOptionsCollection.Add(ToolbarOptions.NumberList);
            ToolbarOptionsCollection.Add(ToolbarOptions.BulletList);
            ToolbarOptionsCollection.Add(saveButon);
            ToolbarOptionsCollection.Add(shareButon);
            ToolbarOptionsCollection.Add(ocrBton);
        }

        private async void ShareAction() {

            Application.Current.Properties[AppConstant.Text] = PlainText;

            Application.Current.Properties[AppConstant.HtmlText] = HtmlText;

            Application.Current.Properties[AppConstant.NoteName] = Note.Name;

            await PopupNavigation.Instance.PushAsync(new SharePopUpPage());

        }

        private async void OcrAction() {
            await TakePhotoAsync();
        }

        public override async void AppearAction() {

            Note = Application.Current.Properties[AppConstant.SelectedNote] as Note;

            var NewNote = await App.FirebaseService.ReadByIdAsync(AppConstant.Notes, Note.Id) as Note;

            if (!string.IsNullOrEmpty(NewNote.FileLocation)) {
                HtmlText = await App.AzureService.GetBlobStorage($"{NewNote.Name}");
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

            await App.FirebaseService.UpdateNoteFileLocationAsync(Note.Id, location);

            File.Delete(filePath);
        }

        async Task TakePhotoAsync() {
            try {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");

                if (PhotoPath != null) {
                   var url = await App.AzureService.UploadToAzureBlobStorage(PhotoPath, PhotoName);
                    if (!string.IsNullOrEmpty(url)) {
                        var txt = await ComputerVision.ReadText(VisionClient, url);
                        var sb = new StringBuilder();
                        if (txt != null) {
                            using (UserDialogs.Instance.Loading("test")) {
                                foreach (var r in txt) {
                                    foreach (var l in r.Lines) {
                                       sb.Append(l.Text);
                                    }
                                }

                                await Application.Current.MainPage.DisplayAlert("", sb.ToString(), "OK");
                            }
                        }
                
                    }
                   
                }
            } catch (FeatureNotSupportedException) {
                // Feature is not supported on the device
            } catch (PermissionException) {
                // Permissions not granted
            } catch (Exception ex) {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        async Task LoadPhotoAsync(FileResult photo) {
            // canceled
            if (photo == null) {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
            PhotoName = photo.FileName;
        }
    }
}
