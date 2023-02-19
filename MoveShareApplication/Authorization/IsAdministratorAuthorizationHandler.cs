using MoveShareApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace MoveShareApplication.Authorization
{
    public class IsAdministratorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Item>
    {
        public IsAdministratorAuthorizationHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Item resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            //userManager.IsInRole(user.Id, "Admin")
            if (context.User.IsInRole(Constants.AdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}