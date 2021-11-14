using Realms;
using System;

namespace Contracts.RepositoryContracts.Notes
{
    public class NotesContract : RealmObject
    {
        [PrimaryKey]
        public int NotesId { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    }
}
