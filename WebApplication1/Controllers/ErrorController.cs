using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorController
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            var  msg = TempData["ErrorMsg"] ?? null;
            switch (statusCode)
            {
                case 403:
                    ViewBag.ErrorMessage = "Sorry, you are not authorized to view this content.";
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page could not be found.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "An internal server error occurred.";
                    break;
                default:
                    ViewBag.ErrorMessage = msg ?? "An error occurred.";
                    break;

            }
            return View();
        }

    }
}
