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
    
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        
        // GET: Users
        [Authorize(Roles ="Administrator, User")]
        public ActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {

                var users = from user in db.Users
                            orderby user.UserName
                            select user;
                ViewBag.UsersList = users;
                ViewBag.UsersListCount = users.Count();
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }
            }
            else
            {
                if (User.IsInRole("User"))
                {
                    string currentUserId = User.Identity.GetUserId();
                    var users = from user in db.Users
                                where user.Id==currentUserId
                                select user;
                    ViewBag.UsersList = users;
                    ViewBag.UsersListCount = users.Count();
                    if (TempData.ContainsKey("message"))
                    {
                        ViewBag.message = TempData["message"].ToString();
                    }
                }
            }
            return View();
        }


        //afisare un utilizator
        [Authorize(Roles ="Administrator, User")]
        public ActionResult Show(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                ApplicationUser user = db.Users.Find(id);
                ViewBag.utilizatorCurent = User.Identity.GetUserId();
                ViewBag.userName = user.UserName;
                ViewBag.User = user;
                string currentRole = user.Roles.FirstOrDefault().RoleId;
                var userRoleName = (from role in db.Roles
                                    where role.Id == currentRole
                                    select role.Name).First();

                ViewBag.roleName = userRoleName;
                return View(user);
            }
            else
            {
                if (User.IsInRole("User"))
                {

                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    ViewBag.utilizatorCurent = User.Identity.GetUserId();
                    ViewBag.userName = user.UserName;
                    ViewBag.User = user;
                    string currentRole = user.Roles.FirstOrDefault().RoleId;
                    var userRoleName = (from role in db.Roles
                                        where role.Id == currentRole
                                        select role.Name).First();

                    ViewBag.roleName = userRoleName;
                    return View(user);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa editati acest profil";
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        

        [Authorize(Roles = "Administrator, User")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }


        [NonAction]
        [Authorize(Roles = "Administrator, User")]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            if (User.IsInRole("Administrator"))
            {
                var selectList = new List<SelectListItem>();
                var roles = from role in db.Roles select role;
                foreach (var role in roles)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = role.Id.ToString(),
                        Text = role.Name.ToString()

                    });
                }
                return selectList;
            }
            else
            {
                var selectList = new List<SelectListItem>();
                var roles = from role in db.Roles where role.Name=="User" select role;
                foreach (var role in roles)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = role.Id.ToString(),
                        Text = role.Name.ToString()

                    });
                }
                return selectList;
            }
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


        [Authorize(Roles = "Administrator, User")]
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;

            string currentUserId = User.Identity.GetUserId();
            if (User.IsInRole("Administrator") || currentUserId == id)
            {


                try
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    if (TryUpdateModel(user))
                    {
                        user.UserName = newData.UserName;
                        user.FirstName = newData.FirstName;
                        user.LastName = newData.LastName;
                        user.Description = newData.Description;
                        user.Email = newData.Email;
                        user.PhoneNumber = newData.PhoneNumber;
                        var roles = from role in db.Roles select role;
                        foreach (var role in roles)
                        {
                            UserManager.RemoveFromRole(id, role.Name);
                        }

                        var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));

                        UserManager.AddToRole(id, selectedRole.Name);

                        db.SaveChanges();

                        if (selectedRole.Name == "Doctor")
                        {
                            //Doctor doctor = new Doctor();
                            //doctor.UserId = id;
                            //doctor.Departments = GetAllDepartments();
                            //doctor.Locations = GetAllLocations();
                            //doctor.User = user;
                            //Console.WriteLine("doctor nou");
                            //db.Doctors.Add(doctor);
                            //Console.WriteLine("adaugat in baza");
                            //db.SaveChanges();
                            //Console.WriteLine("baza salvata");
                            //var selectedDoc= from doc in db.Doctors where doc.UserId == id 
                            //     select doc.DoctorId;
                            //int docId = selectedDoc.FirstOrDefault();
                            //Console.WriteLine(docId);

                            ViewBag.user = id;
                            ViewBag.appUser = user;
                            return RedirectToAction("New", "Doctors", new { userId = id });
                        }
                        else
                        {
                            if (User.IsInRole("Administrator"))
                            {
                                TempData["message"] = "Profilul a fost editat cu succes";

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["message"] = "Profilul a fost editat cu succes";

                                return RedirectToAction( "Index", "Home");
                            }
                        }

                    }
                    else
                    {
                        return View(newData);
                    }


                }
                catch (Exception e)
                {
                    Response.Write(e.Message);

                    newData.Id = id;
                    return View(newData);
                }
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati acest profil";
                return RedirectToAction("Index");
            }
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }
    }
}