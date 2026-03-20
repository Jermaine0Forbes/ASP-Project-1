using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly AppDBContext _context;

    public HomeController(AppDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public IActionResult Index(string? id)
    {
            User? user = null;
            
            if (id != null)
            {
                user =  _context.Users
                .FirstOrDefault(m => m.Id == id);
            }


            if (user == null)
            {
              return View();
            }

            return View(user);

        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public async Task<IActionResult> Users()
    {
        var userData = await _context.Users.ToListAsync();

        return View(userData);
    }


    public async Task<IActionResult> Profile()
    {
        return RedirectToAction("Profile","Account");
    }
}
