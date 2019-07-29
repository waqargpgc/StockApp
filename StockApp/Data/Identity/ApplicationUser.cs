using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Data.Identity
{
    public class ApplicationUser : IdentityUser
    { 
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }


        public string Designation { get; set; }
        public string FullName { get; set; }
        public string Configuration { get; set; }

        [StringLength(450)]
        public string SharedKey { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        //public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }  
        //public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }  
    }
}
