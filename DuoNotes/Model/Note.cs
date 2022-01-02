namespace DuoNotes.Model {
    public class Note : Notebook {

        public new string Id { get; set; }

        public string NotebookId { get; set; }

        public string FileLocation { get; set; }
    }
}
