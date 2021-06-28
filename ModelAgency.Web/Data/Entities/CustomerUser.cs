using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelAgency.Web.Data.Entities {
    public class CustomerUser : ApplicationUser {
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string Address { get; set; }

        [MaxLength(64)]
        public string PostalCode { get; set; }

        [MaxLength(32)]
        public string Country { get; set; }

        [MaxLength(128)]
        public string ImgPath { get; set; }
    }
}
