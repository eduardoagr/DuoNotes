
using Newtonsoft.Json;

using PropertyChanged;

using System;

namespace DuoNotes.Model {

    [AddINotifyPropertyChangedInterface]
    public class User {

        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Email { get; set; }


        [OnChangedMethod(nameof(OnPropertyChanged))]
        public string Password { get; set; }


        [JsonIgnore]
        public Action OnAnyPropertiesChanged { get; set; }
        private void OnPropertyChanged() {
            OnAnyPropertiesChanged?.Invoke();
        }
    }
}
