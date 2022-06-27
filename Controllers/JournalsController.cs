using MedOffice.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class JournalsController : Controller
    {

        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Journals
        public ActionResult Index()
        {
            string usId = User.Identity.GetUserId();
            var journals = from jurns in db.Journals
                           where jurns.UserId == usId
                           select jurns;
            ViewBag.journals = journals;
            return View();
        }

        public ActionResult New()
        {
           
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="User")]
        public ActionResult New(Journal journal)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.user = userId;
            journal.UserId = userId;
            journal.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                db.Journals.Add(journal);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Jurnalul nu a fost adaugat";
                return View(journal);
            }
        }

    }
}