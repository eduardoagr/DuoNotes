using System;
using System.Collections.Generic;
using System.Text;

namespace DuoNotes.Model {
    public interface IElementProperties {

        string Id { get; set; }

        string Name { get; set; }

        string YearOfCreation { get; set; }
    }
}
