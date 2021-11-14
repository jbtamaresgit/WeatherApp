using Contracts.RepositoryContracts.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers.Notes
{
    public class NotesManager : INotesManager
    {
        public NotesManager()
        {}


        public void AddNote()
        {
            throw new NotImplementedException();
        }

        public IQueryable<NotesContract> GetCurrentMonthNotes(DateTime currMonth)
        {
            throw new NotImplementedException();
        }
    }
}
