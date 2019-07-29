using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity
{
    [Table("AppClaims")]
    public class ApplicationClaim 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Type { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(100)]
        public string Description { get; set; }


        public ApplicationClaim()
        { } 

        public ApplicationClaim(string name, string value, string groupName, string description = null)
        {
            Type = CustomClaimTypes.Permission; 
            Name = name; 
            Value = value;
            GroupName = groupName;
            Description = description;
        }

        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationClaim claim)
        {
            return claim.Value;
        } 

        //[StringLength(450)]
        //public string SharedKey { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        //[MaxLength(256)]
        //public string CreatedBy { get; set; }

        //[MaxLength(256)]
        //public string UpdatedBy { get; set; }

        //public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public bool OnlyForOwner { get;  set; }
    }
}
