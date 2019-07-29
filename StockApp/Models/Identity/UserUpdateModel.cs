using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Identity
{
    public class UserUpdateModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }  

        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<string> Claims { get; set; } = new List<string>();
    }
}
