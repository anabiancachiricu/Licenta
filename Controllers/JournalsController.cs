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
            return View();
        }

        public ActionResult New()
        {
            Journal journal = new Journal();
            journal.Date = DateTime.Now;
            journal.UserId= User.Identity.GetUserId();
            return View(journal);
        }

        [HttpPost]
        public ActionResult New(Journal journal)
        {
            
            if (ModelState.IsValid)
            {

                db.Journals.Add(journal);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Locatia nu a fost adaugata";
                return View(journal);
            }
        }

    }
}