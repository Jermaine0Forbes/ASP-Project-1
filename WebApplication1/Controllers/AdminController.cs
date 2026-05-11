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

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)
        {
            _context = context;
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
            select new {post.Title, post.Views, user.UserName }).Take(100).ToListAsync();

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
            return View(await _context.Users.ToListAsync());
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
