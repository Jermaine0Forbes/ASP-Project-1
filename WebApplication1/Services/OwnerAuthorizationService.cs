
// using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
// using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Services
{
public class OwnerAuthorizationRequirement: IAuthorizationRequirement
{
}

public static class AuthorizedRoles
    {
        private readonly static string[] roles = ["Manager", "Admin"];

        public static string[] GetRoles()
        {
            return roles;
        }

        public static string GetFlatRoles()
        {
            return String.Join(",", roles);
        }


    }



public class UserOwnerHandler : AuthorizationHandler<OwnerAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OwnerAuthorizationRequirement requirement, 
            string resource = ""
            )
        {
            ArgumentNullException.ThrowIfNull(context,"context");


            var roles = AuthorizedRoles.GetRoles();
            foreach( var role in roles)
            {
                if(context.User.IsInRole(role))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                
            }
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if(userId != null && userId?.Value == resource)
            {
                 context.Succeed(requirement);
            }

             return Task.CompletedTask;
        }
        
    }
    
}