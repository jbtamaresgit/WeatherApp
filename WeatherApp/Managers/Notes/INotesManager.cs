using Contracts.RepositoryContracts.Notes;
using System;
using System.Linq;

namespace Managers.Notes
{
    public interface INotesManager
    {
        IQueryable<NotesContract> GetCurrentMonthNotes(DateTime currMonth);
        void AddNote();
    }
}
