using Repository.Database;

namespace Repository.Repositories
{
    public class BaseRepository
    {
        protected IDatabaseService DatabaseService;

        public BaseRepository(IDatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }
    }
}
