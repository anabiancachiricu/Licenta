using MedOffice.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Appointments
        public ActionResult Index()
        {
            var UsId= User.Identity.GetUserId();
            var app = from appointments in db.Appointments
                      where appointments.UserId == UsId
                      select appointments;
            ViewBag.AppointmentsList = app;
            return View();
        }


        [NonAction]
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


        public IEnumerable<SelectListItem> GetAllDoctors()
        {
            var selectList = new List<SelectListItem>();
            var docs = (from doctors in db.Doctors
                       select doctors).ToList();
            
            foreach (var doc in docs)
            {
               
                selectList.Add(new SelectListItem
                {
                    Value = doc.DoctorId.ToString(),

                    Text = doc.DepartmentId.ToString()

                }); ;
               
            }
            return selectList;
        }

        public ActionResult New()
        {
            Appointment appointment = new Appointment();
            appointment.Departments = GetAllDepartments();
            appointment.Locations = GetAllLocations();
            appointment.Doctors = GetAllDoctors();
            appointment.UserId = User.Identity.GetUserId();
            appointment.User = db.Users.Find(appointment.UserId);
            return View(appointment);
        }

        [HttpPost]
        public ActionResult New(Appointment appointment)
        {

            try
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                TempData["message"] = "Programarea a fost adaugata!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(appointment);
            }
        }

    }
}