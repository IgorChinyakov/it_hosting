using System;
using System.Collections.Generic;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class Repository
    {
        public Repository()
        {
            Branches = new HashSet<Branch>();
            Collaborators = new HashSet<Collaborator>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<Collaborator> Collaborators { get; set; }
    }
}
