using MedOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Controllers
{
    public class DepartmentsController : Controller
    {

        private ApplicationDbContext db = ApplicationDbContext.Create();


        // GET: Departments
        public ActionResult Index()
        {
            var departments = from department in db.Departments
                            select department;
            ViewBag.departmentsList = departments;
            ViewBag.DepartmentsCount = departments.Count();
            return View();
        }
        public ActionResult New()
        {
            
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="Administrator")]
        public ActionResult New(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Departamentul nu a fost adaugat";
                return View(department);
            }
        }

        public ActionResult Edit(int id)
        {
            Department department = db.Departments.Find(id);
            ViewBag.Department = department;
            return View(department);
        }

        [HttpPut]
        [Authorize(Roles ="Administrator")]
        public ActionResult Edit(int id, Department requestDepartment)
        {
            try
            {
                Department department = db.Departments.Find(id);
                if (TryUpdateModel(department))
                {
                    department.DepartmentName = requestDepartment.DepartmentName;
                    department.DepartmentDescription = requestDepartment.DepartmentDescription;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult Show(int id)
        {
           Department department= db.Departments.Find(id);
           ViewBag.Department = department;
            

            return View();
        }

    }
}