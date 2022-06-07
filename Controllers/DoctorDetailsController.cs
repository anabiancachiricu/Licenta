using MedOffice.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class DoctorDetailsController : Controller
    {

        private ApplicationDbContext db = ApplicationDbContext.Create();

        //// GET: DoctorDetails
        //public ActionResult Index()
        //{
        //    var users = from user in db.Doctors
        //                orderby user.DoctorId
        //                select user;
        //    ViewBag.UsersList = users;

        //    return View();
        //}
        [Authorize(Roles = "User,Administrator,Doctor")]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            ViewBag.Description = user.Description;
            ViewBag.UserId = id;
            ViewBag.User = user;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            string currentId = User.Identity.GetUserId();
            
            var users = from usr in db.Users
                        orderby usr.UserName
                        select usr;
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(
                    us => us.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(usr => userIds.Contains(usr.Id));
                ViewBag.CountUsers = users.Count();
            }
            else
            {
                ViewBag.CountUsers = 0;
            }


            ViewBag.UsersList = users;
            return View();
        }

        [Authorize(Roles = "User,Administrator,Doctor")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
           
            ViewBag.Profile = from profile in db.Doctors
                              where profile.UserId == id
                              select profile;
            ViewBag.User = user;
            ViewBag.Description = db.Users.Find(User.Identity.GetUserId()).Description;
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.CurrentUserName = db.Users.Find(User.Identity.GetUserId()).UserName;
            string currentId = User.Identity.GetUserId();
           
            ViewBag.isAdmin = false;
            if (User.IsInRole("Admin"))
            {
                ViewBag.isAdmin = true;
            }
            
            
            var users = from usr in db.Users
                        orderby usr.UserName
                        select usr;
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<string> userIds = db.Users.Where(
                    us => us.UserName.Contains(search)).Select(u => u.Id).ToList();
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(usr => userIds.Contains(usr.Id));
                ViewBag.CountUsers = users.Count();
            }
            else
            {
                ViewBag.CountUsers = 0;
            }


            ViewBag.UsersList = users;


            return View(user);
        }

        [Authorize(Roles = "User,Administrator,Doctor")]
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            return View(user);
        }

        [HttpPut]
        [Authorize(Roles = "User,Administrator,Doctor")]
        public ActionResult Edit(ApplicationUser requestUser)
        {
            string id = User.Identity.GetUserId();
            try
            {
                ApplicationUser user = db.Users.Find(id);
                if (TryUpdateModel(user))
                {
                    user.UserName = requestUser.UserName;
                    user.PhoneNumber = requestUser.PhoneNumber;
                    user.Description = requestUser.Description;
                    user.UserPhoto = requestUser.UserPhoto;
                    db.SaveChanges();
                    TempData["message"] = "Profilul a fost editat cu succes";

                    


                    return RedirectToAction("Index");

                }
                return View(requestUser);
            }
            catch (Exception e)
            {
                return View(requestUser);
            }
        }

    }
}