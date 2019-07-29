using Data.Identity;
using StockApp.Data.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Identity
{
    public class UserInfoModel : Audit
    {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Designation { get; set; }
            public IEnumerable<RoleListModel> Roles { get; set; } = new List<RoleListModel>(); 
            public IEnumerable<ApplicationClaim> Claims { get; set; } = new List<ApplicationClaim>();
    }
}
