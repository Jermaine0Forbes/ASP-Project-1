using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class LoginController : Controller
{

    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

    [Route("Register")]
    public IActionResult Register()
    {
        return View("../Register/Index");
    }

    [HttpPost]
    public IActionResult RegisterUser(UserModel userData)
    {
        if (ModelState.IsValid)
        {
            

        }
        
        _logger.LogInformation("inside RegisterUser "+userData.Username);
        // _logger.LogInformation(userData.toString());
        return RedirectToAction("Index","Home", userData);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
