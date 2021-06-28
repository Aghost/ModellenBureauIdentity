using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelAgency.Web.Data;
using ModelAgency.Web.Data.Entities;

namespace ModelAgency.Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class ApprovalsModel : PageModel {
        private readonly ApplicationDbContext dbContext;

        public List<ApplicationUser> Users { get; set; }

        public ApprovalsModel(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            Users = dbContext.Users.Where(user => user.AccountStatus == AccountStatus.Pending).ToList();
        }

        public IActionResult OnPostApprove(string email) {
            var user = dbContext.Users.First(user => user.Email == email);
            if(user != null) {
                user.AccountStatus = AccountStatus.Accepted;
                dbContext.SaveChanges();
            }

            return LocalRedirect("/Approvals");
        }

        public IActionResult OnPostReject(string email) {
            var user = dbContext.Users.First(user => user.Email == email);
            if (user != null) {
                user.AccountStatus = AccountStatus.Rejected;
                dbContext.SaveChanges();
            }

            return LocalRedirect("/Approvals");
        }
    }
}
