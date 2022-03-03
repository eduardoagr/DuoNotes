﻿using DuoNotes.Constants;
using DuoNotes.Model;

using System;
using System.IO;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class EditorPageModel : NotesPageModel {

        public Note Note { get; set; }

        public Command SaveCommand { get; set; }

        public string HtmlText { get; set; }

        public EditorPageModel() {


            SaveCommand = new Command(SaveAction);
        }

        public override void AppearAction() {

            Note = Application.Current.Properties[AppConstant.SelectedNote] as Note;
        }


        private async void SaveAction() {

            //Upload to Azure, with a unique name, and get the location

            var LocalFolder = FileSystem.AppDataDirectory;

            var FilePath = Path.Combine(LocalFolder, $"{Note.Name}.rtf");

            var FullPah = Path.GetFullPath(FilePath);

            using (StreamWriter sw = new StreamWriter(FullPah)) {

                sw.WriteLine(HtmlText);
            }
            
            await App.AzureServices.UploadToAzureBlobStorage(FilePath, FullPah);


        }
    }
}
