using System.Threading.Tasks;

namespace DuoNotes.Interfaces {
    public interface IShare {
        Task Show(string title, string messge, string filePath, string ext);
    }
}
