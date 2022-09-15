using System.Collections.Generic;

using DuoNotes.Model;

namespace DuoNotes.Services
{
    public class ShareServices
    {

        internal static List<ShareOptions> GetOptions()
        {

            return new List<ShareOptions> {
                new ShareOptions { Order = 1, ImageName = "word.svg", ImageText = "Word" },
                new ShareOptions { Order = 2, ImageName = "document.svg", ImageText = "Pdf"  },
                new ShareOptions { Order = 3, ImageName = "text.svg", ImageText = "Text"  }
            };
        }
    }
}
