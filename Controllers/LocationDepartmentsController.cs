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
            var locdeps =( from locationDepartments in db.LocationDepartments
                              select locationDepartments).ToList();
            var search = "";

            ViewBag.LocationDepartments = locdeps;
            ViewBag.LocationDepartmentsCount = locdeps.Count();

            var locss = (from locs in db.Locations select locs).ToList();
            ViewBag.locs = locss;
            IList<LocationDepartment> locDepList = new List<LocationDepartment>();

            foreach(var locdep in locdeps)
            {
                Location loc = db.Locations.Find(locdep.LocationId);
                Department dep = db.Departments.Find(locdep.DepartmentId);
                locDepList.Add(new LocationDepartment { Location=loc, Department=dep});

            }

            ViewBag.LocDeps = locDepList;

            if(Request.Params.Get("search")!=null)
            {
                search = Request.Params.Get("search").Trim();
                List<int> LocDepIds = db.LocationDepartments.Where(
                    ld => ld.Department.DepartmentName.Contains(search) 
                    )
                    .Select(ld => ld.LocDepId).ToList();
               
                ViewBag.LocationDepartmentsCount = locss.Count();

            }
            


            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public List<LocationDepartment> getSearchLocDeps(string keyword)
        {
            var docIds = (from doc in db.LocationDepartments.Include("Location").Include("Department")
                          orderby doc.LocDepId
                          where doc.Department.DepartmentName.Contains(keyword)
                          select doc).ToList();
            return docIds;
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

                var exist = from locDep in db.LocationDepartments
                            where locDep.DepartmentId == locationDepartment.DepartmentId
                            && locDep.LocationId == locationDepartment.LocationId
                            select locDep;


                if (exist.Count()==0)
                {
                    db.LocationDepartments.Add(locationDepartment);
                    db.SaveChanges();
                    TempData["message"] = "Combinatia a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["message"] = "Combinatia exista deja!";
                    return RedirectToAction("Index");
                    
                }

               

            }
            catch (Exception e)
            {
                return View(locationDepartment);
            }
        }
    }
}