using System;
using System.Collections.Generic;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class Commit
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int FileId { get; set; }
        public DateTime? CreatingDate { get; set; }

        public virtual File File { get; set; } = null!;
    }
}
