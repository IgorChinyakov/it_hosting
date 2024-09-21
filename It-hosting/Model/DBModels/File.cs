using System;
using System.Collections.Generic;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class File
    {
        public File()
        {
            Commits = new HashSet<Commit>();
        }

        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int BranchId { get; set; }
        public string Title { get; set; } = null!;

        public virtual Branch Branch { get; set; } = null!;
        public virtual ICollection<Commit> Commits { get; set; }
    }
}
