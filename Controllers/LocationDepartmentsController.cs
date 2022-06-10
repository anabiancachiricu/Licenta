using MedOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class LocationDepartmentsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: LocationDepartments
        public ActionResult Index()
        {
            var locdeps = from locationDepartments in db.LocationDepartments
                              select locationDepartments;
            var locs = from locations in db.Locations
                            select locations;
            var deps = from departments in db.Departments
                              select departments;

            ViewBag.LocationDepartments = locdeps;
            ViewBag.LocationDepartmentsCount = locdeps.Count();
            ViewBag.Locations = locs;
            ViewBag.Departments = deps;

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

        [NonAction]
        [Authorize(Roles = "Administrator")]
        public IEnumerable<SelectListItem> GetAllLocations()
        {
            var selectList = new List<SelectListItem>();
            var locs = from locations in db.Locations
                       select locations;
            foreach (var loc in locs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = loc.LocationId.ToString(),
                    Text = loc.Address.ToString()

                });
            }
            return selectList;
        }

        public ActionResult New()
        {
            LocationDepartment locationDepartment = new LocationDepartment();
            locationDepartment.Departments = GetAllDepartments();
            locationDepartment.Locations = GetAllLocations();
            return View(locationDepartment);
        }

        [HttpPost]
        [Authorize(Roles ="Administrator")]
        public ActionResult New(LocationDepartment locationDepartment)
        {

            try
            {
                db.LocationDepartments.Add(locationDepartment);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost adaugat!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(locationDepartment);
            }
        }
    }
}