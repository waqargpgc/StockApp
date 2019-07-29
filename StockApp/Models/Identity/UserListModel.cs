using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Identity
{
    public class UserListModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        //public IEnumerable<string> Roles { get; set; } = new List<string>();
        //public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
    }
}
