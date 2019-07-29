using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Authorization 
{
    public static class AppOperations
    {
        public static OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
        public static OperationAuthorizationRequirement Approve =
          new OperationAuthorizationRequirement { Name = Constants.ApproveOperationName };
        public static OperationAuthorizationRequirement Reject =
          new OperationAuthorizationRequirement { Name = Constants.RejectOperationName };
    }

    public static class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly string ApproveOperationName = "Approve";
        public static readonly string RejectOperationName = "Reject";

        public static readonly string SuperAdminRole = "SuperAdmin";
        public static readonly string AdminRole = "Admin"; 
        public static readonly string ShopUserRole = "ShopUser"; 
        public static readonly string ModeratorRole = "Moderator";

        public static readonly string CustomerRole = "Customer";
        public static readonly string SuplierRole = "Suplier";  
    }

}
