using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace MoveShareApplication.Authorization
{
    public static class MoveShareItemOperation
    {
        public static OperationAuthorizationRequirement Edit =
          new OperationAuthorizationRequirement { Name = Constants.EditOperationName };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string EditOperationName = "EditPost";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly string AdministratorRole = "Administrator";
        public static readonly string UserRole = "User";
    }
}