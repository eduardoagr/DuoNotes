using System.Collections.Generic;

using Xamarin.Forms;

namespace DuoNotes.Services {
    internal class ColorService {
        internal static List<Color> GetColors() {

            return new List<Color>() {
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Orange,
                Color.Purple,
                Color.Violet,
                Color.YellowGreen,
                Color.DarkBlue,
                Color.Chocolate,
                Color.DarkGray,
                Color.DarkOliveGreen,
                Color.DarkRed,
                Color.DarkGreen,
            };
        }
    }
}
