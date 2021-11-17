using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DuoNotes.Utils {

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension {

        const string ResourceId = "DuoNotes.Resources.AppResources";

        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(
            ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider) {
            if (Text == null)
                return "";

            var ci = Thread.CurrentThread.CurrentUICulture;
            var translation = resmgr.Value.GetString(Text, ci);

            if (translation == null) {

#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
				translation = Text;
#endif
            }
            return translation;
        }
    }
}
