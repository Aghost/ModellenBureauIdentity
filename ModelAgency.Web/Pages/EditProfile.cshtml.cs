using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelAgency.Web.Data;
using ModelAgency.Web.Data.Entities;

namespace ModelAgency.Web.Pages
{
    [Authorize(Policy = "PageOwner")]
    public class EditProfileModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHost;

        public ModelUser modelUser { get; set; }
        public string ReturnUrl { get; set; }

        public EditProfileModel(ApplicationDbContext dbContext,
            IWebHostEnvironment webHost) {
            this.dbContext = dbContext;
            this.webHost = webHost;
        }

        public IActionResult OnGet(string id)
        {
            if (!User.IsInRole("Admin") && !User.HasClaim(claim => claim.Value == id))
                return LocalRedirect($"/Profile?id={id}");
            modelUser = dbContext.Models.Include(model => model.Photos).FirstOrDefault(model => model.Id == id);
            if(modelUser == null) {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(string id, string name, DateTime dob) {
            modelUser = dbContext.Models.Include(model => model.Photos).FirstOrDefault(model => model.Id == id);

            modelUser.Name = name;
            modelUser.DOB = dob;
            modelUser.AccountStatus = AccountStatus.Pending;

            dbContext.SaveChanges();
            return Page();
        }

        public IActionResult OnPostAddPhotos(string id, List<IFormFile> photos) {
            modelUser = dbContext.Models.Include(model => model.Photos).FirstOrDefault(model => model.Id == id);
            foreach (var photo in photos) {
                var relativedir = Path.Combine("img", "models", modelUser.Name);
                var dir = Path.Combine(webHost.WebRootPath, relativedir);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                var relative = Path.Combine(relativedir, photo.FileName);
                var path = Path.Combine(webHost.WebRootPath, relative);
                modelUser.Photos.Add(new Photo() { Path = relative });
                using (var file = System.IO.File.Create(path)) {
                    photo.CopyTo(file);
                }
            }

            modelUser.AccountStatus = AccountStatus.Pending;
            dbContext.SaveChanges();

            return Page();
        }

        public IActionResult OnPostDelete(string id, int photoId) {
            modelUser = dbContext.Models.Include(model => model.Photos).FirstOrDefault(model => model.Id == id);
            var photo = modelUser.Photos.FirstOrDefault(photo => photo.Id == photoId);
            if (photo == null)
                return NotFound();
            System.IO.File.Delete(Path.Combine(webHost.WebRootPath, photo.Path));
            modelUser.Photos.Remove(photo);
            dbContext.SaveChanges();
            modelUser.AccountStatus = AccountStatus.Pending;

            return Page();
        }
    }
}
