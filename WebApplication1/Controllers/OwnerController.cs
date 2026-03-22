using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

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
        public virtual async Task<bool> IsPostOwner(string id, int postId , AppDBContext context)
        {

               var post =  context.Posts.Where( p => p.User != null && p.User.Id == id && p.Id == postId ).Count();

            return post == 0;

        }


        public virtual async Task<bool> IsOwner(string id, string userId , AppDBContext context)
        {
            return !(id == userId);
        }
    }

}