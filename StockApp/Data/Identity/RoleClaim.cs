using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Identity
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        //[StringLength(450)]
        //public string SharedKey { get; set; }

        //public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }

        //[MaxLength(256)]
        //public string CreatedBy { get; set; }

        //[MaxLength(256)]
        //public string UpdatedBy { get; set; }

        //public DateTime? UpdatedDate { get; set; }
        //public DateTime? CreatedDate { get; set; }
    }
}
 