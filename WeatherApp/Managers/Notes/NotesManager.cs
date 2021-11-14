using Contracts.RepositoryContracts.Notes;
using Repository.Repositories.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Notes
{
    public class NotesManager : INotesManager
    {
        private readonly INotesRepository NotesRepository;
        public NotesManager(INotesRepository notesRepository)
        {
            NotesRepository = notesRepository;
        }

        public async Task<bool> AddNote(NotesContract test)
        {
            await Task.Run(() =>
            {
                return NotesRepository.Insert(test);
            });

            return false;
        }

        public IEnumerable<NotesContract> GetCurrentMonthNotes(DateTime currDate)
        {
            //return new List<NotesContract>();
            //return NotesRepository.GetList<NotesContract>().Where(x => x.Month.Equals(currMonth.Month) && x.Year.Equals(currMonth.Year));
            var realm = NotesRepository.GetRealm();
            var test = realm.All<NotesContract>().Where(x => x.Month == currDate.Month && x.Year == currDate.Year);
            return test;
        }
    }
}
