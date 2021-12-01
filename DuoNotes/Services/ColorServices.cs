using DuoNotes.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Text;

using Xamarin.Forms;

namespace DuoNotes.Services {
    public class ColorServices {
        public static List<Color> GetItems() {

            return new List<Color>() {
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Magenta,
                Color.Orange,
                Color.Purple,
                Color.Pink,
                Color.CornflowerBlue,
                Color.DarkViolet,
                Color.DarkRed,
                Color.DarkGreen,
                Color.DarkGray,
                Color.OrangeRed,
                Color.GreenYellow
            };
        }
    }
}
