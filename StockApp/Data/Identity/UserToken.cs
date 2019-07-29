using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Identity
{
    public class UserToken : IdentityUserToken<string>
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
