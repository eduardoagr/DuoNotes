
using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Resources;
using DuoNotes.View;
using DuoNotes.View.PopUps;

using PropertyChanged;

using Rg.Plugins.Popup.Services;

using SkiaSharp;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;

using Xamarin.Forms;

namespace DuoNotes.PageModels {

    [AddINotifyPropertyChangedInterface]
    public class NotebooksPageModel {

        public Command ProfileCommnd { get; set; }

        public Command PageAppearCommand { get; set; }

        public Command SelectedItemLongPressCommand { get; set; }

        public Command SwitchVisibilityCommand { get; set; }

        public Command<string> TextToSearchCommand { get; set; }

        public Command<Notebook> DeleteNotebookCommand { get; set; }

        public Command<NotebookNote> SelectedItemCommand { get; set; }

        public Command<Frame> FabAnimationCommmand { get; set; }

        public Command NewNotebboNoteAppearCommand { get; set; }

        public ObservableCollection<NotebookNote> FireBaseNotebookNotes { get; set; }

        public ObservableCollection<NotebookNote> NewFireBaseNotebookNotes { get; set; }

        public Firebase.Auth.User FireUser { get; set; }

        public FontImageSource Glyph { get; set; }

        public bool IsVisible { get; set; }

        public NotebooksPageModel() {

            FireBaseNotebookNotes = new ObservableCollection<NotebookNote>();

            NewFireBaseNotebookNotes = new ObservableCollection<NotebookNote>();

            PageAppearCommand = new Command(AppearAction);

            SelectedItemCommand = new Command<NotebookNote>(SelectedItemActionAsync);

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);

            ProfileCommnd = new Command(NavigateCommandAsync);

            DeleteNotebookCommand = new Command<Notebook>(DeleteNotebookCommandAction);

            SelectedItemLongPressCommand = new Command(SelectedLongPressItemActionAsync);

            SwitchVisibilityCommand = new Command(SwitchVisibilityAction);

            NewNotebboNoteAppearCommand = new Command(NewNotebboNoteAppearAction);

            TextToSearchCommand = new Command<string>(TextToSearchAction);
        }

        public virtual async void TextToSearchAction(string SeachTerm) {

            if (!string.IsNullOrEmpty(SeachTerm)) {

                var FilteredItems = FireBaseNotebookNotes.Where(item =>
                item.Name.ToLowerInvariant().Contains(SeachTerm.ToLowerInvariant())).ToList();

                FireBaseNotebookNotes.Clear();

                foreach (var Item in FilteredItems) {
                    FireBaseNotebookNotes.Add(Item);
                }

            } else {
                FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
                IsVisible = false;
            }
        }
    

        private async void NewNotebboNoteAppearAction() {
            await Application.Current.MainPage.DisplayAlert("", "", "OK");
        }

        public virtual void SwitchVisibilityAction() {

            IsVisible = !IsVisible;
        }

        public virtual async void SelectedLongPressItemActionAsync() {
            string answer = await Application.Current.MainPage.DisplayActionSheet($"{AppResources.Warning}: {AppResources.Edit}",
                null, null,
                AppResources.Yes,
                AppResources.No);

            Console.WriteLine(answer);
        }

        public virtual async void AppearAction() {
            FireUser = await App.FirebaseService.GetProfileInformationAndRefreshTokenAsync();
            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
        }

        private async void NavigateCommandAsync() {
            await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
        }

        public virtual async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            await PopupNavigation.Instance.PushAsync(new NotebookPopUpPage());

        }

        public async virtual void SelectedItemActionAsync(NotebookNote notebookNote) {

            if (notebookNote is NotebookNote notebook) {

                NotesPage notesPage = new NotesPage();
                Application.Current.Properties[AppConstant.SelectedNotebook] = notebook;
                await Application.Current.MainPage.Navigation.PushAsync(notesPage, true);
            }
        }

        public async void DeleteNotebookCommandAction(Notebook obj) {

            string ext = ".html";

            App.FirebaseService.DeleteNotebookNotAsync(obj.Id, AppConstant.Notebooks);

            //We want to read, but we do not want to update
            var Notes = await App.FirebaseService.ReadWithOutUpdateAsync(AppConstant.Notes, obj.Id);
            foreach (var item in Notes) {
                var note = item as Note;
                App.FirebaseService.DeleteNotebookNotAsync(note.Id, AppConstant.Notes);
                App.AzureService.DeleteFileFromBlobStorage($"{note.Name}{ext}");
            }

            FireBaseNotebookNotes = await App.FirebaseService.ReadAsync(AppConstant.Notebooks);
        }
    }
}