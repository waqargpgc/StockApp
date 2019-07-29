using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace App.IdentityServices
{
    public interface ICurrentUserSerivce
    {
        IPrincipal CurrentUser { get; set; }
    }
}
