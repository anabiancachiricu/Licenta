using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MedOffice.Models;

namespace MedOffice.Controllers
{
    public class LocationsController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Locations
       
        //get all locations
        public ActionResult Index()
        {
            var locations = from location in db.Locations
                        select location;
            ViewBag.LocationsList = locations;
            string markers = "[";
            foreach (var loc in locations)
            {
                markers += "{";
                markers += string.Format("'City':'{0}',", loc.City);
                markers += string.Format("'Adress':'{0}',", loc.Address);
                markers += string.Format("'Latitude':'{0}',", loc.Latitude);
                markers += string.Format("'Longitude':'{0}',", loc.Longitude);
                markers += "}";
            }
            markers += "]";
            ViewBag.Markers = markers;
            return View();
        }


        public ActionResult Show(int LocationId)
        {
            Location location = db.Locations.Find(LocationId);
            ViewBag.Location = location;
            ViewBag.Departments = location.Departments;

            var countDepartments = location.Departments.Count();
            ViewBag.CountDepartments = countDepartments;

            return View();
        }

        public ActionResult New()
        {
            var departments = from dep in db.Departments select dep;
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public ActionResult New(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Locatia nu a fost adaugata";
                return View(location);
            }
        }


        public ActionResult Edit(int id)
        {
            Location location = db.Locations.Find(id);
            ViewBag.Location = location;
            ViewBag.Departments = location.Departments;
            var departments = from dep in db.Departments select dep;
            
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Location requestLocation)
        {
            try
            {
                Location location = db.Locations.Find(id);
                if (TryUpdateModel(location))
                {
                    location.City = requestLocation.City;
                    location.Address = requestLocation.Address;
                    location.Latitude = requestLocation.Latitude;
                    location.Longitude = requestLocation.Longitude;
                    location.Departments = requestLocation.Departments;

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }


    }
}