
using System.Collections.Generic;
using System.Security.Claims;
using StockApp.Data.Enitities;

namespace Data.Identity
{
    public class UserModel : Audit
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>(); 
    }
}
  