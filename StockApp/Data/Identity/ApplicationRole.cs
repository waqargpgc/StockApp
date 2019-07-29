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
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string role) : base(role)
        {

        }

        public ApplicationRole()
        {

        }

        //public virtual IList<IdentityUserRole<string>> Users { get; set; } = new List<IdentityUserRole<string>>();
        //public virtual IList<IdentityRoleClaim<string>> Claims { get; set; } = new List<IdentityRoleClaim<string>>();
        [StringLength(450)]
        public string SharedKey { get; set; }
         
        [StringLength(450)]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
