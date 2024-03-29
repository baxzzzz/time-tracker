using System.Threading.Tasks;
using TimeTracking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace TimeTracking.Authorization
{
    public abstract class AdministratorsAuthorizationHandler<T>
                    : AuthorizationHandler<OperationAuthorizationRequirement, T>
    {
        private UserManager<IdentityUser> _userManager;

        public AdministratorsAuthorizationHandler(UserManager<IdentityUser> 
            userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                              OperationAuthorizationRequirement requirement, 
                                              T resource)
        {
            if (context.User == null)
            {
                return;
            }

            // Administrators can do anything.
            var userIdentity = await _userManager.GetUserAsync(context.User);
            
            if (userIdentity != null && await _userManager.IsInRoleAsync(userIdentity, Constants.AdministratorsRole))
            {
                context.Succeed(requirement);
            }        
        }
    }
}