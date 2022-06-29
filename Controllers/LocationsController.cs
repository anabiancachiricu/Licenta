using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MedOffice.Models;
using GoogleMaps.LocationServices;

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
            ViewBag.LocationsCount = locations.Count();

            //string markers = "[";
            //foreach (var loc in locations)
            //{
            //    markers += "{";
            //    markers += string.Format("'City':'{0}',", loc.City);
            //    markers += string.Format("'Adress':'{0}',", loc.Address);
            //    markers += string.Format("'Latitude':'{0}',", loc.Latitude);
            //    markers += string.Format("'Longitude':'{0}',", loc.Longitude);
            //    markers += "}";
            //}
            //markers += "]";
            //ViewBag.Markers = markers;

            return View();
        }


        public ActionResult Show(int id)
        {
            Location location = db.Locations.Find(id);
            ViewBag.Location = location;
            //ViewBag.Latitude = Math.Truncate(location.Latitude).ToString() +"."+(location.Latitude - Math.Truncate(location.Latitude)).ToString();
            //ViewBag.Longitude = Math.Truncate(location.Longitude).ToString() + "." + (location.Longitude - Math.Truncate(location.Longitude)).ToString(); 
            //return View();


            var locationService = new GoogleLocationService("AIzaSyAGiM9eKTPSUqdPUGJQUIQS7b4A9pmGCvw");
            var point = locationService.GetLatLongFromAddress(location.Address);

            location.Latitude = point.Latitude;
            location.Longitude = point.Longitude;

            string lati = (location.Latitude - Math.Truncate(location.Latitude)).ToString().Split(',').Last();
            ViewBag.Latitude = Math.Truncate(location.Latitude).ToString() + "." + lati;

            string longi= (location.Longitude - Math.Truncate(location.Longitude)).ToString().Split(',').Last();
            ViewBag.Longitude = Math.Truncate(location.Longitude).ToString() + "." + longi;

            return View();

        }

        public ActionResult New()
        {
            //List<SelectListItem> items = new List<SelectListItem>();

           
            //foreach (var dep in departments)
            //{
            //    items.Add(new SelectListItem
            //    {
            //        Text = dep.DepartmentName.ToString(),
            //        Value=dep.DepartmentId.ToString()
            //    });
               
            //}

           
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult New(Location location)
        {
            var locationService = new GoogleLocationService("AIzaSyAGiM9eKTPSUqdPUGJQUIQS7b4A9pmGCvw");
            var point = locationService.GetLatLongFromAddress(location.Address);

            location.Latitude = point.Latitude;
            location.Longitude = point.Longitude;

            if (ModelState.IsValid)
            {

                db.Locations.Add(location);

                //foreach (SelectListItem item in items)
                //{
                //    if (item.Selected)
                //    {
                //        Department dep = db.Departments.Find(item.Value);
                //        location.Departments.Add(dep);
                //    }
                //}

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
           
            return View(location);
        }

        [HttpPut]
        [Authorize(Roles ="Administrator")]
        public ActionResult Edit(int id, Location requestLocation)
        {
            Location location = db.Locations.Find(id);
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();

                if (TryUpdateModel(location))
                {
                    location.City = requestLocation.City;
                    location.Address = requestLocation.Address;
                    location.Latitude = requestLocation.Latitude;
                    location.Longitude = requestLocation.Longitude;

                    db.SaveChanges();

                }
                return RedirectToAction("Index");
                
                
            }
            catch (Exception e)
            {
                return View(requestLocation);
            }
        }


    }
}