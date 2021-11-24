using System;

namespace DuoNotes.Model {
    public class Note : IElementProperties {

        public string NotebookId { get; set; }

        public string FileLocation { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string YearOfCreation { get; set; }
    }
}
