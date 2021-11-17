using System;

namespace DuoNotes.Model {
    public class Notes {

        public string Id { get; set; }

        public string NotebookId { get; set; }

        public string Name { get; set; }

        public string FileLocation { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
