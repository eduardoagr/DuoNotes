﻿using DuoNotes.Constants;
using DuoNotes.Model;

using Syncfusion.XForms.RichTextEditor;

using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    public class EditorPageModel : NotesPageModel {

        public Note Note { get; set; }

        public Command SaveCommand { get; set; }

        public EditorPageModel() {


            SaveCommand = new Command(SaveAction);
        }

        public override void AppearAction() {

            Note = Application.Current.Properties[AppConstant.SelectedNote] as Note;


        }

        private void SaveAction() {

            //Upload to Azure, with a unique name, and get the location
        }

    }
}
