using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _email;

        public AdminController(AppDBContext context, UserManager<User> userManager, EmailService email)
        {
            _context = context;
            _userManager = userManager;
            _email = email;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {

            var addresses = await GetDailyData("[IpAddresses]");
            var views = await _context.IpAddresses.Take(100)
            .Select(ip => new { ip.Address, ip.Path, ip.CreatedAt, ip.UserId, ip.Zip })
            .OrderByDescending(m => m.CreatedAt).ToListAsync();
            var dailyPosts = await GetDailyData("[Posts]");
            var topPosts = await (from user in _context.Users
                                  join post in _context.Posts on user.Id equals post.UserId
                                  select new { post.Title, post.Views, user.UserName }).Take(100).ToListAsync();

            ViewBag.Addresses = addresses;
            ViewBag.Views = views;
            ViewBag.Posts = dailyPosts;
            ViewBag.TopPosts = topPosts;
            return View();
        }

        public async Task<List<DailyDataViewModel>> GetDailyData(string table)
        {
            string query = $@"

            WITH Hours12 AS (
                SELECT 0 AS Hour UNION ALL
                SELECT Hour + 1 FROM Hours12 WHERE Hour < 11
            ),
            AllHours AS (
                SELECT Hour,
                    CASE Hour WHEN 0 THEN '12 AM' ELSE CAST(Hour AS VARCHAR) + ' AM' END AS Label
                FROM Hours12
                UNION ALL
                SELECT Hour + 12,
                    CASE Hour WHEN 0 THEN '12 PM' ELSE CAST(Hour AS VARCHAR) + ' PM' END AS Label
                FROM Hours12
            ),
            HourlyData AS (
                SELECT 
                    DATEPART(HOUR, CreatedAt)   AS Hour,
                    COUNT(*) AS Num
                FROM {table}
                WHERE CreatedAt  >= CAST(GETDATE() AS DATE)
                AND CreatedAt  <  DATEADD(DAY, 1, CAST(GETDATE() AS DATE))
                GROUP BY DATEPART(HOUR, CreatedAt)
            )
            SELECT 
                A.Hour,
                A.Label,
                ISNULL(D.Num, 0) AS Num
            FROM AllHours A
            LEFT JOIN HourlyData D ON A.Hour = D.Hour
            ORDER BY A.Hour;
            ";

            return await _context.Database.SqlQueryRaw<DailyDataViewModel>(query).ToListAsync();

        }


        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            // var users = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
            var users =
            await (
                from user in _context.Users
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                join role in _context.Roles on userRole.RoleId equals role.Id
                select (
                    new UserListViewModel
                    {
                        Id = user.Id,
                        CreatedAt = user.CreatedAt,
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = role.Name
                    })

            ).ToListAsync();

            ViewBag.UserList = users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit(string id)
        {
            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            var user = await _context.Users
             .FirstOrDefaultAsync(m => m.Id == id);

            var role = string.Join(",", await _userManager.GetRolesAsync(user!));

            var roles = await _context.Roles
            .Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name,
                Selected = r.Name != null && r.Name.Contains(role)
            }).ToListAsync();

            var data = new UserEditViewModel()
            {
                User = user,
                Roles = roles,
                Role = role,
            };
            ViewBag.Categories = roles;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = model.User?.Id;
                var exists = UserExists(id ?? "");
                if (id == null || !exists) { return NotFound(); }

                var user = await _userManager.FindByIdAsync(id);
                var role = await _context.Roles.Where(r => r.Name == model.Role).FirstOrDefaultAsync();

                if (user == null || role == null) { throw new Exception("Cannot find user or role when attempting to edit"); }
                var oldrole = await _context.UserRoles.Where(ur => ur.UserId == user.Id).FirstAsync();


                user.UserName = model.User != null ? model.User.UserName : user.UserName;
                user.Email = model.User != null ? model.User.Email : user.Email;
                user.PhoneNumber = model.User != null ? model.User.PhoneNumber : user.PhoneNumber;
                user.TwoFactorEnabled = model.User != null ? model.User.TwoFactorEnabled : user.TwoFactorEnabled;
                user.EmailConfirmed = model.User != null ? model.User.EmailConfirmed : user.EmailConfirmed;
                user.LockoutEnabled = model.User != null ? model.User.LockoutEnabled : user.LockoutEnabled;

                var newrole = new UserRole()
                {
                    User = user,
                    Role = role,
                };

                _context.Update(user);
                //    await _context.UserRoles.AddAsync(newrole);
                _context.Remove(oldrole);
                _context.Add(newrole);

                await _context.SaveChangesAsync();

            }
            else
            {
                ModelState.AddModelError("invalid", "a specific form value is invalid");
            }

            return RedirectToAction("UserEdit");
        }

        [HttpGet]
        public async Task<IActionResult> UserCreate()
        {
            var roles = await _context.Roles
            .Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name,

            }).ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UserCreate(UserCreateViewModel model)
        {


            if (ModelState.IsValid)
            {

                var (theUser, theRole) = model;
                DateTime currentDateTime = DateTime.Now;
                DateTime futureDateTime = DateTime.Now.AddDays(5);
                User user = new()
                {
                    Email = theUser.Email,
                    UserName = theUser.UserName ?? "",
                    PhoneNumber = theUser.PhoneNumber ?? "",
                    CreatedAt = currentDateTime,
                    OtpExpirationDate = futureDateTime,
                };

                var role = await _context.Roles.Where(r => r.Name == theRole).FirstOrDefaultAsync() ?? throw new Exception("Role hasn't been chosen or doesn't exist");
                var doesNameExist = await _userManager.FindByNameAsync(user.UserName!);

                if (doesNameExist != null)
                {
                    ModelState.AddModelError("", "the user name is not unique");
                    return View(model);
                }

                if(theUser.PasswordHash == null) { throw new Exception("Password is empty");}
                if(theRole == null) {}

                var result = await _userManager.CreateAsync(user, theUser.PasswordHash);
                string? token = null;

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role.Name!);
                    // 1. Generate Email Confirmation Token
                    token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // 2. Create confirmation link
                    var confirmationLink = Url.Action("VerifyingEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    var cem = new ConfirmationEmailModel()
                    {
                        UserName = user.UserName,
                        Title = "Confirmation Email",
                        Url = confirmationLink ?? "",
                    };

                    if(user.Email == null){throw new Exception("Email is empty");}

                    _email.Send(cem, "ConfirmationEmail", user.Email);

                }
                else
                {
                    throw new Exception("User was not created");
                }


            }
            else
            {
                ModelState.AddModelError("invalid", "a specific form value is invalid");
            }
            return RedirectToAction("Users");
        }


        // GET: Admin/Settings
        public async Task<IActionResult> Settings()
        {
            return View(await _context.Users.ToListAsync());
        }


        // GET: Admin/Account
        public async Task<IActionResult> Account()
        {
            return View(await _context.Users.ToListAsync());
        }


        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
