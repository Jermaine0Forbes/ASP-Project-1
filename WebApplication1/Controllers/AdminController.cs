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

            string query = @"
            WITH Hours AS (
                SELECT 0 AS Hour
                UNION ALL
                SELECT Hour + 1 FROM Hours WHERE Hour < 23
            ),
            HourlyData AS (
                SELECT 
                    DATEPART(HOUR, CreatedAt) AS Hour,
                    COUNT(*)                   AS Num
                FROM [IpAddresses]
                WHERE CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
                GROUP BY DATEPART(HOUR, CreatedAt)
            )
            SELECT 
                H.Hour,
                ISNULL(D.Num, 0) AS Num
            FROM Hours H
            LEFT JOIN HourlyData D ON H.Hour = D.Hour
            ORDER BY H.Hour;
            ";

            var addresses = await _context.IpAddresses.FromSqlRaw(query).ToListAsync();
            ViewBag.Addresses = addresses;
            return View(await _context.Users.ToListAsync());
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


        // GET: Admin/Settings
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
