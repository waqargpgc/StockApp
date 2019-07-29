using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Identity
{
    public class RoleCreateModel 
    {
        [Required, MaxLength(100)]
        public string RoleName { get; set; }
        public IEnumerable<string> Claims { get; set; } 
    }
}
  