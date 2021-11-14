using Realms;
using System;

namespace Contracts.RepositoryContracts.Notes
{
    public class NotesContract : RealmObject
    {
        [PrimaryKey]
        public int NotesId { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    }
}
