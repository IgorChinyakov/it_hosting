using System;
using System.Collections.Generic;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class User
    {
        public User()
        {
            Collaborators = new HashSet<Collaborator>();
            Repositories = new HashSet<Repository>();
        }

        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ICollection<Collaborator> Collaborators { get; set; }
        public virtual ICollection<Repository> Repositories { get; set; }
    }
}
