using System.Collections.Generic;

using Xamarin.Forms;

namespace DuoNotes.Services {
    public class ColorServices {
        public static List<Color> GetItems() {

            return new List<Color>() {
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.DarkGray,
                Color.Orange,
                Color.Purple,
            };
        }
    }
}
