using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Identity
{
    public class RoleUpdateModel
    {
        public string Id { get; set; }

        [Required, StringLength(450)]
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}
 