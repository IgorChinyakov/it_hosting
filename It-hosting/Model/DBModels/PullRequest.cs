using System;
using System.Collections.Generic;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class PullRequest
    {
        public int Id { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }

        public virtual Branch FromBranch { get; set; } = null!;
        public virtual Branch ToBranch { get; set; } = null!;
    }
}
