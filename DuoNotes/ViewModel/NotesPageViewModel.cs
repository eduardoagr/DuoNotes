﻿using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View.PopUps;
using DuoNotes.ViewModel.PopUps;

using Rg.Plugins.Popup.Services;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {

    public class NotesPageViewModel : MainPageViewModel {


        readonly FirebaseServices Services;

        public new Command<Frame> FabAnimationCommmand { get; set; }


        public Notebook RecivedSelectedNotebook { get; set; }

        public Note SeletedNote { get; set; }

        public NotesPageViewModel() {

            Services = App.services;

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            MessagingCenter.Subscribe<MainPageViewModel, Notebook>(this, App.NotebookID, (sender, value) => {
                RecivedSelectedNotebook = value;
                Services.ReadAsync(App.Notes, RecivedSelectedNotebook.Id);
                MessagingCenter.Unsubscribe<MainPageViewModel, Notebook>(this, App.NotebookID);
            });



        }

        private async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Wait a moment
            await Task.Delay(100);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            var notesPopUp = new NotesPopUp();
            await PopupNavigation.Instance.PushAsync(notesPopUp);
            var viewModel = notesPopUp.BindingContext as NewNotePopUpViewModel;
            viewModel.RecivedSelectedNotebookAccion(RecivedSelectedNotebook);
        }

        public override async void SeletedItemActionAsync() {
            base.SeletedItemActionAsync();

            await Application.Current.MainPage.DisplayAlert(String.Empty, "I have to do my own implementation", "OK");

        }
    }
}
