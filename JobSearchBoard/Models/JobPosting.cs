using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchBoard.Models
{
    public class JobPosting
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Salary { get; set; }
        public string Company { get; set; }

        public IdentityUser User { get; set; }

    }
}
