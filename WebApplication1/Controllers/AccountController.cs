// using Azure.Identity;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace WebApplication1.Controllers
{
    public class AccountController
    (SignInManager<User> signInManager,
    UserManager<User> userManager,
    AppDBContext context,
    IAuthorizationService authorizationService,
    EmailService email
    ) : OwnerController(authorizationService)
    {
        private readonly SignInManager<User> signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly UserManager<User> userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        private readonly AppDBContext _context = context;

        private readonly EmailService _email = email;


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [EnableRateLimiting("LoginPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "username or password is incorrect.");
                    return View(model);
                }
                if (result.IsNotAllowed)
                {
                    Console.WriteLine("is not allowed");
                }

                result = await userManager.FindByNameAsync(model.UserName);

                if(result.UpdatedAt < DateTime.Now )
                {
                   token =  await userManager.GenerateTwoFactorAsync(result, TokenOptions.DefaultEmailProvider);
                  var dem = new DefaultEmailModel()
                  {
                    UserName = result.UserName,
                    Title = "One Time Password",
                    Description = "Here is your password "+token,
                    Url = ""  
                  };

                   _email.Send(dem, "DefaultEmail", result.Email);
                    TempData["UserId"] = result.Id;
                    return RedirectToAction("VerifyOtp");
                }

                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }

            return View(model);
        }


        public async Task<IActionResult> VerifyOtp()
        {
            
            View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {


                DateTime currentDateTime = DateTime.Now;
                User user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName ?? "",
                    CreatedAt = currentDateTime,
                };

                var doesNameExist = await userManager.FindByNameAsync(user.UserName!);

                if (doesNameExist != null)
                {
                    ModelState.AddModelError("", "the user name is not unique");
                    return View(model);
                }

                var result = await userManager.CreateAsync(user, model.Password);
                IdentityResult? role = null;
                string? token = null;

                if (result.Succeeded)
                {
                    role = await userManager.AddToRoleAsync(user, "User");
                    // 1. Generate Email Confirmation Token
                    token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    // 2. Create confirmation link
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    var cem = new ConfirmationEmailModel()
                    {
                        UserName = user.UserName,
                        Title = "Confirmation Email",
                        Url = confirmationLink ?? "",
                    };

                    _email.Send(cem, "ConfirmationEmail", user.Email);

                }
                if (role != null && role.Succeeded && token != null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.userId!);
                if (user == null || model.token == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return RedirectToAction("Error", "Home");
                }


                var result = await userManager.ConfirmEmailAsync(user, model.token);
                if (!result.Succeeded)
                {
                     return RedirectToAction("Error", "Home");
                }

                await signInManager.SignInAsync(user, false, "Password");

                return View("VerifyEmail");

            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email!);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword!);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            //Need to re migrate files so that I can retreive the posts through collection
            var posts = user != null
            ? await _context.Posts.Where(p => p.User != null && p.User.Id.Equals(user.Id)).ToListAsync()
            : [];

            return View(posts);
        }
    }
}