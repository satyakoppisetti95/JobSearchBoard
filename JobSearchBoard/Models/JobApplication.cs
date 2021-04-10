using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchBoard.Models
{
    public class JobApplication
    {
        public int ID { get; set; }
        public int JobPostingID { get; set; }
        public DateTime AppliedDateTime { get; set; }
        public IdentityUser User { get; set; }

        public virtual JobPosting JobPosting { get; set; }

    }
}
