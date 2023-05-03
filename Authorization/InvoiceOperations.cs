using Microsoft.AspNetCore.Authorization.Infrastructure;
namespace IdentityApp.Authorization
{
    public class InvoiceOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };

        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Constants.ReadOperartionName };

        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Constants.UpdateOperartionName };

        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };

        public static OperationAuthorizationRequirement Approve =
            new OperationAuthorizationRequirement { Name = Constants.ApprovedOperartionName };

        public static OperationAuthorizationRequirement Reject =
            new OperationAuthorizationRequirement { Name = Constants.RejectedOperartionName };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperartionName = "Read";
        public static readonly string UpdateOperartionName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly string ApprovedOperartionName = "Approved";
        public static readonly string RejectedOperartionName = "Rejected";

        public static readonly string InvoiceManagersRole = "InvoiceManager";
        public static readonly string InvoiceAdminRole = "InvoiceAdmin";



    }
}
