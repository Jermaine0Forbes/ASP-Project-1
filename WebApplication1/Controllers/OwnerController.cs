using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public abstract class OwnerController(IAuthorizationService authorizationService) : Controller
    {
        protected  readonly IAuthorizationService _authorizationService = authorizationService;

        public virtual async Task<bool> IsOwnerOrAuthorized(string id)
        {

            var authorizationResult = await _authorizationService.AuthorizeAsync(
            User, // Current user principal
            id, // The resource ID to check against (the profile's owner ID)
            "IsOwnerOrAuthorized"); // The policy name

            return !authorizationResult.Succeeded;

        }
        public virtual async Task<bool> IsOwner(string id)
        {

            var authorizationResult = await _authorizationService.AuthorizeAsync(
            User, // Current user principal
            id, // The resource ID to check against (the profile's owner ID)
            "IsOwner"); // The policy name

            return !authorizationResult.Succeeded;

        }
    }

}