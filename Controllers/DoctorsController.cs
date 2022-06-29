using MedOffice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class DoctorsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

       
        public ActionResult Index1()
        {
            var docs = (from doc in db.Doctors.Include("Department").Include("Location").Include("User")
                        group doc by doc.DepartmentId into docDepGroup
                        where docDepGroup.Count()>=1
                        orderby docDepGroup.Key
                        select docDepGroup).ToList();
            ViewBag.DoctorsDepList = docs;
            ViewBag.DoctorsDepCount = docs.Count();
            return View();
        }

        public ActionResult Index2()
        {
            var docs = (from doc in db.Doctors.Include("Department").Include("Location").Include("User")
                        group doc by doc.LocationId into docLocGroup
                        where docLocGroup.Count() >= 1
                        orderby docLocGroup.Key
                        select docLocGroup).ToList();
            ViewBag.DoctorsLocList = docs;
            ViewBag.DoctorsLocCount = docs.Count();
            return View();
        }

        // GET: Doctors
        public ActionResult Index()
        {
            var docs = (from doc in db.Doctors.Include("Department").Include("Location").Include("User")
                       select doc).ToList();
            ViewBag.DoctorsList = docs;
            ViewBag.DoctorsCount = docs.Count();

            IList<Doctor> DocsList = new List<Doctor>();

            foreach (var doc  in docs)
            {
                Location loc = db.Locations.Find(doc.LocationId);
                Department dep = db.Departments.Find(doc.DepartmentId);
                ApplicationUser us = db.Users.Find(doc.UserId);
                DocsList.Add(new Doctor { Location = loc, Department = dep , User=us});

            }
            ViewBag.docs = DocsList;

            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();
                List<int> docsIds = db.Doctors.Include("User").Where(
                    dcs => dcs.User.UserName.Contains(search)).Select(u => u.DoctorId).ToList();
                docs = (db.Doctors.Where(dc => docsIds.Contains(dc.DoctorId))).ToList();
                ViewBag.CountDoctors = docs.Count();
            }
            else
            {
                ViewBag.CountDoctors = 0;
            }
            ViewBag.DoctorsList = docs;
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public List<Doctor> getSearchDoctors(string keyword)
        {
            var docIds = (from doc in db.Doctors.Include("User")
                           orderby doc.DoctorId
                           where doc.User.UserName.Contains(keyword)
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

        public ActionResult New(string userId)
        {
            Doctor doctor = new Doctor();
            doctor.Departments = GetAllDepartments();
            doctor.Locations = GetAllLocations();
            ViewBag.user = userId;
            
            //doctor.User = ViewBag.appUser;
            return View(doctor);
        }

        [HttpPost]
        public ActionResult New(Doctor doctor)
        {

            if (ModelState.IsValid)
            {

                db.Doctors.Add(doctor);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Doctorul nu a fost adaugat";
                return View(doctor);
            }
        }



        [Authorize(Roles = "Doctor, User")]
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);

            return View(user);
        }

        [Authorize(Roles = "Doctor, User")]
        [HttpPut]
        public ActionResult Edit(ApplicationUser newData)
        {
            string id = User.Identity.GetUserId();

           
            


            try
            {
                ApplicationUser user = db.Users.Find(id);

                //byte[] imageData = null;
                //if (Request.Files.Count > 0)
                //{
                //    HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                //    using (var binary = new BinaryReader(poImgFile.InputStream))
                //    {
                //        imageData = binary.ReadBytes(poImgFile.ContentLength);
                //    }
                //}
                ////Here we pass the byte array to user context to store in db    
                //user.UserPhoto = imageData;

                if (TryUpdateModel(user))
                {
                    
                    user.UserName = newData.UserName;
                    user.FirstName = newData.FirstName;
                    user.LastName = newData.LastName;
                    user.Description = newData.Description;
                    user.PhoneNumber = newData.PhoneNumber;
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


        public FileContentResult DoctorPhoto(int docId)
        {
            var doc = (from docs in db.Doctors.Include("User") where docs.DoctorId == docId select docs).ToList().First();
            if(doc.User.UserPhoto==null)
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");
            }
            else
            {
                return new FileContentResult(doc.User.UserPhoto, "image/jpeg");
            }

        }
        
        [Authorize(Roles="Administrator, User, Doctor")]
        public ActionResult Show(int id)
        {
           
            Doctor doctor = db.Doctors.Find(id);
            ViewBag.doctor = doctor;
            ApplicationUser user = db.Users.Find(doctor.UserId);
            ViewBag.user = user;
            Department department = db.Departments.Find(doctor.DepartmentId);
            ViewBag.department = department;
            Location location = db.Locations.Find(doctor.LocationId);
            ViewBag.location = location;

            return View(doctor);

        }

    }
}