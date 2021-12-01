using Newtonsoft.Json;

using PropertyChanged;

using System;

namespace DuoNotes.Model {

    [AddINotifyPropertyChangedInterface]
    public class NotebookNote {
        public string UserID { get; set; }

        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Name { get; set; }

        public string CreatedDate { get; set; }

        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Color { get; set; }

        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Desc { get; set; }

        [JsonIgnore]
        public Action OnAnyPropertiesChanged { get; set; }
        private void OnPropertyChanged() {
            OnAnyPropertiesChanged?.Invoke();
        }
    }
}
