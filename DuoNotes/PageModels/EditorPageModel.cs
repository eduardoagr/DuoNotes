﻿
using DuoNotes.Constants;
using DuoNotes.Fonts;
using DuoNotes.Model;
using DuoNotes.Pages.PopUps;

using Rg.Plugins.Popup.Services;

using Syncfusion.XForms.RichTextEditor;

using System.Collections.ObjectModel;
using System.IO;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class EditorPageModel : NotesPageModel {

        public Note Note { get; set; }

        public Command OcrCommand { get; set; }

        public ObservableCollection<object> ToolbarOptionsCollection { get; set; }

        public string HtmlText { get; set; }

        public string PlainText { get; set; }

        public EditorPageModel() {

            ToolbarOptionsCollection = new ObservableCollection<object>();

            AddToolbarItems();
        }

        public void AddToolbarItems() {

            //Insert a new item to the custom toolbar collection.
            var ocrBton = new Button {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50,
                WidthRequest = 50,
                Text = MaterialIcons.MagnifyExpand,
                FontFamily = "ma",
                FontSize = 22,
                Command = new Command(OcrAction)
            };
            var saveButon = new Button {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50,
                WidthRequest = 50,
                Text = MaterialIcons.ContentSave,
                FontFamily = "ma",
                FontSize = 22,
                Command = new Command(SaveAction)
            };
            var shareButon = new Button {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50,
                WidthRequest = 50,
                Text = MaterialIcons.ShareVariant,
                FontFamily = "ma",
                FontSize = 22,
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

        private void OcrAction() {
            System.Console.WriteLine("frefrefre ");
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
    }
}
