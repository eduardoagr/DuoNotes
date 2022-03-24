using DuoNotes.Constants;
using DuoNotes.Interfaces;
using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.Services;

using Rg.Plugins.Popup.Services;

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

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

            HtmlText = Application.Current.Properties[AppConstant.Text] as string;

            NoteName = Application.Current.Properties[AppConstant.NoteName] as string;
        }

        private async void ShareOptionsAction() {

            var share = DependencyService.Get<IShare>();
            var localFolder = FileSystem.AppDataDirectory;
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;

            if (Option.Order == 1) {

                string fileName = $"{NoteName}.docx";

                var filePath = Path.Combine(localFolder, fileName);

                Stream inputStream = assembly.GetManifestResourceStream(HtmlText);

                using (WordDocument document = new WordDocument()) {

                    document.EnsureMinimal();

                    StreamReader streamReader = new StreamReader(inputStream);
                    string HtmlText = streamReader.ReadToEnd();

                    document.LastParagraph.AppendHTML(IgnoreVoidElementsInHTML(HtmlText));

                    document.Open(inputStream, FormatType.Html);
                    //Creates an instance of memory stream
                    using (MemoryStream memoryStream = new MemoryStream()) {

                        document.Save(memoryStream, FormatType.Docx);

                        //Saves the Word document to file stream.
                        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                            memoryStream.WriteTo(fs);
                        }
                        document.Save(memoryStream, FormatType.Docx);
                    }
                    //Closes the document
                    document.Close();
                }

                await App.AzureService.UploadToAzureBlobStorage(filePath, fileName);

            } else if (Option.Order == 2) {

                string fileName = $"{NoteName}.pdf";

                var file = Path.Combine(localFolder, fileName);

                //Creates an instance of WordDocument Instance (Empty Word Document)

                WordDocument document = new WordDocument();

                //Add a section & a paragraph in the empty document

                document.EnsureMinimal();

                //Append HTML string to Word document paragraph.

                document.LastParagraph.AppendHTML(IgnoreVoidElementsInHTML(HtmlText));

                //Instantiation of DocIORenderer for Word to PDF conversion

                DocIORenderer render = new DocIORenderer();

                //Converts Word document into PDF document

                PdfDocument pdf = render.ConvertToPDF(document);

                //Releases all resources used by the Word document and DocIO Renderer objects

                render.Dispose();

                document.Close();

                //Saves the PDF file

                MemoryStream outputStream = new MemoryStream();

                pdf.Save(outputStream);

                //Closes the instance of PDF document object

                await share.Show(AppResources.NoteTite, NoteName, file, "pdf");

                File.Delete(file);

            } else {
                await ShareText(PlainText);
            }
        }

        public async Task ShareText(string text) {
            await Share.RequestAsync(new ShareTextRequest {
                Text = text
            });
        }

        private void ClosePopUpAction() {
            PopupNavigation.Instance.PopAsync();
        }

        /// <summary>
        /// Correcting void elements in the HTML. 
        /// </summary>
        /// <param name="inputString">HTML string with void HTML elements</param>
        /// <returns></returns>
        private string IgnoreVoidElementsInHTML(string inputString) {
            inputString = inputString.Replace("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\">", "");
            inputString = inputString.Replace("<br>", "<br/>");
            inputString = inputString.Replace("\n", "");
            inputString = inputString.Replace("\r", "");
            inputString = inputString.Replace("<title></title>", "");
            inputString = inputString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE html PUBLIC" +
                " \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");
            return inputString;
        }

    }
}
