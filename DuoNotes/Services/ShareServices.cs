using DuoNotes.Model;

using System.Collections.Generic;

namespace DuoNotes.Services {
    public class ShareServices {

        internal static List<ShareOptions> GetOptions() {

            return new List<ShareOptions> {
                new ShareOptions { Order = 1, ImageName = "word.svg" },
                new ShareOptions { Order = 2, ImageName = "document.svg" },
                new ShareOptions { Order = 3, ImageName = "text.svg" }
            };
        }
    }
}
