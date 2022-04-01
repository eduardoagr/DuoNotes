using DuoNotes.Constants;
using DuoNotes.Interfaces;
using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.Utils;

using Rg.Plugins.Popup.Services;

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

using System.Collections.Generic;
using System.IO;

using Xamarin.Essentials;
using Xamarin.Forms;


namespace DuoNotes.PageModels.PopUps {
    public class SharePopUpPageModel {

        public Command DismissPopUpCommand { get; set; }

        public Command ShareOptionsCommand { get; set; }

        public Command PageAppearCommand { get; set; }

        public List<ShareOptions> ShareOptions { get; set; }

        public ShareOptions Option { get; set; }

        public string NoteName { get; set; }

        public string HtmlText { get; set; }

        public string PlainText { get; set; }

        public SharePopUpPageModel() {

            DismissPopUpCommand = new Command(ClosePopUpAction);

            ShareOptions = ShareServices.GetOptions();

            ShareOptionsCommand = new Command(ShareOptionsAction);

            PageAppearCommand = new Command(PageAppearAction);
        }

        private void PageAppearAction() {

            PlainText = Application.Current.Properties[AppConstant.Text] as string;

            HtmlText = Application.Current.Properties[AppConstant.HtmlText] as string;

            NoteName = Application.Current.Properties[AppConstant.NoteName] as string;
        }

        private async void ShareOptionsAction() {

            IShare share = DependencyService.Get<IShare>();

            var localFolder = FileSystem.CacheDirectory;

            if (Option.Order == 1) {

                var ext = ".docx";

                string fileName = $"{NoteName}{ext}";

                var filePath = Path.Combine(localFolder, fileName);

                using (WordDocument document = new WordDocument()) {

                    document.EnsureMinimal();

                    document.LastParagraph.AppendHTML(HtmlValidator.IgnoreVoidElementsInHTML(HtmlText));

                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                        document.Save(fs, FormatType.Docx);
                    }
                }

                await share.Show(NoteName, "htllo", filePath, ext);

            } else if (Option.Order == 2) {

                string fileName = $"{NoteName}.pdf";

                var filePath = Path.Combine(localFolder, fileName);

                using (WordDocument document = new WordDocument()) {

                    document.EnsureMinimal();

                    document.LastParagraph.AppendHTML(HtmlValidator.IgnoreVoidElementsInHTML(HtmlText));                  

                    using (DocIORenderer render = new DocIORenderer()) {

                        using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                            using (PdfDocument pdf = render.ConvertToPDF(document)) { pdf.Save(fs); }
                        }
                    }
                }
                await App.AzureService.UploadToAzureBlobStorage(filePath, fileName);

                await Share.RequestAsync(new ShareFileRequest {
                    File = new ShareFile(filePath)
                });

               //File.Delete(filePath);

            } else {
                await Share.RequestAsync(new ShareTextRequest {
                    Text = PlainText,
                });
            }

            await PopupNavigation.Instance.PopAsync();
        }
        private void ClosePopUpAction() {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
