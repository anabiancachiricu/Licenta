﻿using MedOffice.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Appointments
        [Authorize(Roles ="Doctor, User")]
        public ActionResult Index()
        {
            var UsId= User.Identity.GetUserId();
            if(User.IsInRole("User"))
            {
                var app = (from appointments in db.Appointments.Include("User").Include("Doctor")
                          where appointments.UserId == UsId
                          orderby appointments.DateTime
                          select appointments).ToList();
                ViewBag.AppointmentsList = app;
            }
            else if(User.IsInRole("Doctor"))
            {
                var doc = (from docs in db.Doctors.Include("User").Include("Department").Include("Location")
                          where docs.UserId == UsId
                          select docs).ToList();
                int docId = doc.FirstOrDefault().DoctorId;
                var app = (from appointments in db.Appointments.Include("User").Include("Doctor")
                          where appointments.DoctorId == docId
                          orderby appointments.DateTime
                          select appointments).ToList();
                ViewBag.AppointmentsList = app;

            }
            
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

        [HttpDelete]
        [Authorize(Roles = "Doctor")]
        public ActionResult Delete(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            string currentuserId = User.Identity.GetUserId();
            var doc = (from docs in db.Doctors where docs.UserId == currentuserId select docs).ToList();
            int docId = doc.FirstOrDefault().DoctorId;

            string notificationBody = "<p> Programarea dumneavoastra a fost anulata de doctor. <p>";
            notificationBody += "<p> Data la care erati programat:" + appointment.DateTime.Date + "</p>";
            notificationBody += "<p> Ora la care erati programat:" + appointment.DateTime.Hour + "</p>";
            notificationBody += "<p>Ne cerem scuze pentru disconfortul creat!</p>";

            ApplicationUser currUser = db.Users.Find(appointment.UserId);
            string sendTo = currUser.Email;

            if (appointment.DoctorId == docId)
            {
                db.Appointments.Remove(appointment);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                SendEmailNotification(sendTo, "Programarea dvs a fost anulata", notificationBody);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu puteti sterge o programare care nu va apartine";
                return RedirectToAction("Index");
            }
        }


        private void SendEmailNotification(string toEmail, string subject, string content )
        {
            const string senderEmail = "anabiancachiricu@gmail.com";
            const string senderPassword = "Ph13fje$$";
            const string smtpServer = "smtp.gmail.com";
            const int smtpPort = 587;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage email = new MailMessage(senderEmail, toEmail, subject, content);
            email.IsBodyHtml = true;
            email.BodyEncoding = UTF8Encoding.UTF8;

            try
            {
                System.Diagnostics.Debug.WriteLine("Sending email...");
                smtpClient.Send(email);
                System.Diagnostics.Debug.WriteLine("Sent!");
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error occured ");
                System.Diagnostics.Debug.WriteLine(e.Message.ToString());
            }
        }
    }
}