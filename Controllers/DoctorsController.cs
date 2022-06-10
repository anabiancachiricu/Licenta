using MedOffice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class DoctorsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Doctors
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            var docs = from doc in db.Doctors
                       select doc;
            ViewBag.DoctorsList = docs;
            ViewBag.DoctorsCount = docs.Count();
            
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

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            doctor.Departments = GetAllDepartments();
            doctor.Locations = GetAllLocations();

            ViewBag.DocDep = doctor.Departments.FirstOrDefault();
            ViewBag.DocLoc = doctor.Locations.FirstOrDefault();

            return View(doctor);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Doctor newData)
        {
            Doctor doctor = db.Doctors.Find(id);
            
           
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
              
                if (TryUpdateModel(doctor))
                {

                    doctor.DepartmentId = newData.DepartmentId;
                    doctor.LocationId = newData.LocationId;
                    db.SaveChanges();


                    return RedirectToAction("Index");
                   
                }
                else
                {
                    return View(newData);
                }

            }
            catch (Exception e)
            {
                Response.Write(e.Message);

                return View(newData);
            }
        }


    }
}