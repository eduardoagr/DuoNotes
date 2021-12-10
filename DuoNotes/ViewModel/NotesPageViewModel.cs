using DuoNotes.Model;
using DuoNotes.View.PopUps;

using Rg.Plugins.Popup.Services;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DuoNotes.ViewModel {
    public class NotesPageViewModel : MainPageViewModel {

        public new Command<Frame> FabAnimationCommmand { get; set; }

        public NotesPageViewModel() {

            App.services.ReadAsync("Notes");

            FabAnimationCommmand = new Command<Frame>(AnimateButtonCommand);
        }

        private async void AnimateButtonCommand(Frame obj) {

            await obj.ScaleTo(0.8, 50, Easing.Linear);
            //Wait a moment
            await Task.Delay(100);
            //Scale to normal
            await obj.ScaleTo(1, 50, Easing.Linear);
            await PopupNavigation.Instance.PushAsync(new NotesPopUp());
        }
    }
}
