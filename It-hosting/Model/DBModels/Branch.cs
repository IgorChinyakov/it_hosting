using System;
using System.Collections.Generic;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class Branch
    {
        public Branch()
        {
            Files = new HashSet<File>();
            PullRequestFromBranches = new HashSet<PullRequest>();
            PullRequestToBranches = new HashSet<PullRequest>();
        }

        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public string Title { get; set; } = null!;
        public bool? IsMain { get; set; }

        public virtual Repository Repository { get; set; } = null!;
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<PullRequest> PullRequestFromBranches { get; set; }
        public virtual ICollection<PullRequest> PullRequestToBranches { get; set; }
    }
}
