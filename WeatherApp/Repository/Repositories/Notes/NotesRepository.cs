using Contracts.RepositoryContracts.Notes;
using Repository.Database;

namespace Repository.Repositories.Notes
{
    public class NotesRepository : Repository<NotesContract>, INotesRepository
    {
        public NotesRepository(IDatabaseService db) : base(db)
        {

        }
    }
}
