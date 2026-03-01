using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class LoginController : Controller
{

    private readonly ILogger<LoginController> _logger;
    private JavaScriptEncoder _javaScriptEncoder;

    public LoginController(ILogger<LoginController> logger, JavaScriptEncoder jsEncoder)
    {
        _logger = logger;
        _javaScriptEncoder = jsEncoder;
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
        // _logger.LogInformation(_javaScriptEncoder.Encode(userData.Username));
        // _logger.LogInformation(userData.toString());
        return RedirectToAction("Index","Home", userData);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
