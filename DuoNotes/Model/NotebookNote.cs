using Newtonsoft.Json;

using PropertyChanged;

using System;

namespace DuoNotes.Model {

    [AddINotifyPropertyChangedInterface]
    public class NotebookNote {

        public string NotebookId { get; set; }

        public string UserID { get; set; }

        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Name { get; set; }

        public string CreatedDate { get; set; }

        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Color { get; set; }

        [JsonIgnore]
        public Action OnAnyPropertiesChanged { get; set; }
        private void OnPropertyChanged() {
            OnAnyPropertiesChanged?.Invoke();
        }
    }
}
