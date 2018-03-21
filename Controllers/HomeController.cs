using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using weddingPlanner.Models;
using System.Linq;

namespace weddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingContext _context;

        public HomeController(WeddingContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                return RedirectToAction("Success", "Dashboard");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User ExistingUser = _context.Users.SingleOrDefault(user => user.Email == model.Email);
                if (ExistingUser != null)
                {
                    ViewBag.Message = "User with this email already exists!";
                    return View("Index");
                }
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Add(NewUser);
                _context.SaveChanges();
                NewUser = _context.Users.SingleOrDefault(user => user.Email == NewUser.Email);
                HttpContext.Session.SetInt32("UserId", NewUser.UserId);
                HttpContext.Session.SetString("FirstName", NewUser.FirstName);
                ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
                return RedirectToAction("Success", "Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        

        [HttpPost]
        [Route("LogUser")]
        public IActionResult LogUser(string Email, string Password)
        {
            User FoundUser = _context.Users.SingleOrDefault(user => user.Email == Email && user.Password == Password);
            if (FoundUser == null)
            {
                ViewBag.Message = "Login failed.";
                return View("Index");
            }
            else
            {
                HttpContext.Session.SetInt32("UserId", FoundUser.UserId);
                HttpContext.Session.SetString("FirstName", FoundUser.FirstName);
                ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
                return RedirectToAction("Success", "Dashboard");
            }
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
