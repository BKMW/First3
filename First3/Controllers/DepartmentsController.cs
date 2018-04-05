using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using First3.Models;

namespace First3.Controllers
{
    public class DepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Departments
        public ActionResult Index()
        {
           
            return View(db.Departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);

        }
        public ActionResult DetailsPopup(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Department department = db.Departments.Find(id);
            //if (department == null)
            //{
            //    return HttpNotFound();
            //}
            return PartialView(department);

        }
        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Createe()
        {
            var department = new Department();
            return PartialView(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createe([Bind(Include = "DepartmentID,DepartmentName,File")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                department.File.SaveAs(Server.MapPath("~/ImagesDepartments/") + department.DepartmentID + ".jpg");
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName,File")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                department.File.SaveAs(Server.MapPath("~/ImagesDepartments/") + department.DepartmentID + ".jpg");
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName,File")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                if (department.File != null)
                {
                    department.File.SaveAs(Server.MapPath("~/ImagesDepartments/") + department.DepartmentID + ".jpg");
                  
                }
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            try
            {
                db.Departments.Remove(department);
                db.SaveChanges();
            }
            catch
            {

            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        // Get partial view
        public ActionResult EmplyeesByDepartment(ICollection<Employee> employees)
        {

            return PartialView(employees);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
