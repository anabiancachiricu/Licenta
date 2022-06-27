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
                    Text = doc.User.UserName.ToString()
                }) ;

            }
            return selectList;
        }

        public ActionResult New()
        {
            Appointment appointment = new Appointment();
            appointment.Doctors = GetAllDoctors();
            
            //appointment.User = db.Users.Find(appointment.UserId);
            return View(appointment);
        }

        [HttpPost]
        [Authorize(Roles="User")]
        public ActionResult New(Appointment appointment)
        {
            
            appointment.UserId = User.Identity.GetUserId();
            appointment.DateTime = Convert.ToDateTime(Request.Form["DateTime"]);
            if(ModelState.IsValid)
            {

                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Programarea nu a fost adaugata";
                return View(appointment);
            }
        }

    }
}