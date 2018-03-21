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
    public class DashboardController : Controller
    {
        private WeddingContext _context;

        public DashboardController(WeddingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                List<Wedding> AllWeddings = _context.Weddings
                    .Include(wedding => wedding.Creator)
                    .Include(wedding => wedding.RSVPs)
                    .ThenInclude(rsvp => rsvp.User).ToList();
                List<Dictionary<string, object>> WeddingList = new List<Dictionary<string, object>>();
                foreach (Wedding wedding in AllWeddings)
                {
                    bool owned = false;
                    bool RSVPed = false;
                    int RSVPs = 0;
                    if (HttpContext.Session.GetInt32("UserId") == wedding.UserId)
                    {
                        owned = true;
                    }
                    foreach (var rsvp in wedding.RSVPs)
                    {
                        if (rsvp.UserId == HttpContext.Session.GetInt32("UserId"))
                        {
                            RSVPed = true;
                        }
                        ++RSVPs;
                    }
                    Dictionary<string, object> newWeddingEvent = new Dictionary<string, object>();
                    newWeddingEvent.Add("WeddingId", wedding.WeddingId);
                    newWeddingEvent.Add("WedderOne", wedding.WedderOne);
                    newWeddingEvent.Add("WedderTwo", wedding.WedderTwo);
                    newWeddingEvent.Add("WeddingDate", wedding.WeddingDate);
                    newWeddingEvent.Add("Owned", owned);
                    newWeddingEvent.Add("RSVPs", RSVPs);
                    newWeddingEvent.Add("RSVPed", RSVPed);
                    WeddingList.Add(newWeddingEvent);
                }
                ViewBag.Weddings = WeddingList;
                return View("Success", "Dashboard");
            }
            else
            {
                return RedirectToAction("Success", "Dashboard");
            }
        }

        [HttpPost]
        [Route("CreateWedding")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            if (ModelState.IsValid)
            {
                Wedding newWedding = new Wedding
                {
                    WedderOne = wedding.WedderOne,
                    WedderTwo = wedding.WedderTwo,
                    WeddingDate = wedding.WeddingDate,
                    Address = wedding.Address,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    UserId = (int)HttpContext.Session.GetInt32("UserId")
                };
                _context.Weddings.Add(newWedding);
                _context.SaveChanges();
                return RedirectToAction("Success", "Dashboard");
            }
            else
            {
                return View("WeddingForm", wedding);
            }
        }

        [HttpGet]
        [Route("WeddingForm")]
        public IActionResult WeddingForm()
        {
            return View();
        }

        [HttpGet]
        [Route("delete/{WeddingId}")]
        public IActionResult Delete(int WeddingId)
        {
            Wedding deleteWedding = _context.Weddings.SingleOrDefault
                (w => w.UserId == (int)HttpContext.Session.GetInt32("UserId") &&
                w.WeddingId == WeddingId);
            if (deleteWedding != null)
            {
                _context.Weddings.Remove(deleteWedding);
                _context.SaveChanges();
            }
            return RedirectToAction("Success", "Dashboard");
        }

        [HttpGet]
        [Route("Reserve/{WeddingId}")]
        public IActionResult Reserve(int WeddingId)
        {
            RSVP newRSVP = new RSVP
            {
                UserId = (int)HttpContext.Session.GetInt32("UserId"),
                WeddingId = WeddingId
            };
            RSVP existingRSVP = _context.RSVPs.SingleOrDefault
                (r => r.UserId == (int)HttpContext.Session.GetInt32("UserId") &&
                r.WeddingId == WeddingId);
            if (existingRSVP == null)
            {
                _context.RSVPs.Add(newRSVP);
                _context.SaveChanges();
            }
            return RedirectToAction("Success", "Dashboard");
        }

        [HttpGet]
        [Route("UnRSVP/{WeddingId}")]
        public IActionResult UnRSVP(int WeddingId)
        {
            RSVP excuse = _context.RSVPs.SingleOrDefault
                (r => r.UserId == (int)HttpContext.Session.GetInt32("UserId") && 
                r.WeddingId == WeddingId);
            if (excuse != null)
            {
                _context.RSVPs.Remove(excuse);
                _context.SaveChanges();
            }
            return RedirectToAction("Success", "Dashboard");
        }

        [HttpGet]
        [Route("Wedding/{WeddingId}")]
        public IActionResult Wedding(int WeddingId)
        {
            Wedding currWedding = _context.Weddings
                .Include(w => w.RSVPs)
                .ThenInclude(r => r.User)
                .SingleOrDefault(w => w.WeddingId == WeddingId);
            ViewBag.currWedding = currWedding;
            return View("Wedding", "Dashboard");

        }
    }
}