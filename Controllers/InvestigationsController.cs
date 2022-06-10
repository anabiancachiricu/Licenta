using MedOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class InvestigationsController : Controller
    {

        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Investigations
        public ActionResult Index()
        {
            var investigations = from inv in db.Investigations
                                 group inv by inv.DepartmentId into invGroup
                                 where invGroup.Count()>=1
                                 orderby invGroup.Key 
                                 select invGroup;
            ViewBag.Investigations = investigations;
            ViewBag.InvestigationsCount = investigations.Count();
            return View();

        }

        [NonAction]
        [Authorize(Roles = "Administrator")]
        public IEnumerable<SelectListItem> GetAllDepartments()
        {
            var selectList = new List<SelectListItem>();
            var deps = from departments in db.Departments
                       select departments;
            foreach (var dep in deps)
            {
                selectList.Add(new SelectListItem
                {
                    Value = dep.DepartmentId.ToString(),
                    Text = dep.DepartmentName.ToString()

                });
            }
            return selectList;
        }

        public ActionResult New()
        {
            Investigation investigation = new Investigation();
            investigation.Departments = GetAllDepartments();
            return View(investigation);
        }
        [HttpPost]
        public ActionResult New(Investigation investigation)
        {
            try
            {
                db.Investigations.Add(investigation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e )
            {
                
                return View(investigation);
            }
        }

    }
}