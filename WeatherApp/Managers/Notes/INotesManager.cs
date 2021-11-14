using Contracts.RepositoryContracts.Notes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managers.Notes
{
    public interface INotesManager
    {
        IEnumerable<NotesContract> GetCurrentMonthNotes(DateTime currDate);
        Task<bool> AddNote(NotesContract test);
    }
}
