using AutomatedTellerMachine.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomatedTellerMachine.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;

            //Pass the Pin code to the Viewbag
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Pin = user.Pin;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //[ActionName("contact-us")]
        public ActionResult Contact()
        {
            ViewBag.TheMessage = "Having trouble? Send us a message.";

            return View("Contact");
        }

        [HttpPost]        
        public ActionResult Contact(string message)
        {
            ViewBag.TheMessage = "Thanks, we got your message!";
            return PartialView("_ContactThanks");
        }

        [MyLoggingFilter]
        public ActionResult Foo()
        {
            return View("About");
        }

        public ActionResult Serial(string letterCase)
        {
            var serial = "ASPNETMVCATM1";

            if (letterCase == "lower")
                return Content(serial.ToLower());

            //return Content(serial);
            /*return Json(new { name = "serial", value = serial },
                JsonRequestBehavior.AllowGet);*/
            return RedirectToAction("Contact");
        }
    }
}