using Firebase.Database;

using System;

namespace DuoNotes.Model {
    public class Notebook : IElementProperties {
        public string UserID { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string YearOfCreation { get; set; }

    }
}
