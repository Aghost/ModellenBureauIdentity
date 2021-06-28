using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelAgency.Web.Data.Entities {
    public enum AccountStatus
    {
        Accepted,
        Pending,
        Rejected
    }
    public class ApplicationUser : IdentityUser {
        public AccountStatus AccountStatus { get; set; }
    }
}
