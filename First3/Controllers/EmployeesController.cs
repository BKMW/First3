using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using First3.Models;
using System.Linq.Dynamic;
using PagedList;
using OfficeOpenXml;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace First3.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        //public ActionResult Index()
        //{
        //    var employees = db.Employees.Include(e => e.Department);
        //    return View(employees.ToList());
        //}
        // public ActionResult Index(String search = "", int page = 2, int take = 3)
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PositionSortParm = sortOrder == "position" ? "position_desc" : "position";
            ViewBag.DepartmentSortParm = sortOrder == "department" ? "department_desc" : "department";
            ViewBag.LastSortParm = sortOrder == "last" ? "last_desc" : "last";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var employees = db.Employees.Include(e => e.Department);
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.FirstName.Contains(searchString));
            }
            //var skip = take * (page - 1);
            //var employees = db.Employees.Where(e => e.FirstName.Contains(search)).OrderBy(e => e.EmployeeID).Skip(skip).Take(take);
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.FirstName);
                    break;
                case "last":
                    employees = employees.OrderBy(e => e.LastName);
                    break;
                case "last_desc":
                    employees = employees.OrderByDescending(e => e.LastName);
                    break;

                case "position":
                    employees = employees.OrderBy(e => e.Position);
                    break;
                case "position_desc":
                    employees = employees.OrderByDescending(e => e.Position);
                    break;
                case "department":
                    employees = employees.OrderBy(e => e.Department.DepartmentName);
                    break;
                case "department_desc":
                    employees = employees.OrderByDescending(e => e.Department.DepartmentName);
                    break;

                default:
                    employees = employees.OrderBy(e => e.FirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(employees.ToPagedList(pageNumber, pageSize));
        }




        // function get Emplyees

        //
        //public ActionResult Index(int page = 1, string sort = "FirstName", string sortdir = "asc", string search = "")
        //{
        //    int pageSize = 10;
        //    int totalRecord = 0;
        //    if (page < 1) page = 1;
        //    int skip = (page * pageSize) - pageSize;
        //    var data = GetEmployees(search, sort, sortdir, skip, pageSize, out totalRecord);
        //    ViewBag.TotalRows = totalRecord;
        //    ViewBag.search = search;
        //    return View(data);
        //}
        //
        public List<Employee> GetEmployees(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
           
                //var v = (from a in db.Employees
                //         where
                //                 a.FirstName.Contains(search) ||
                //                 a.LastName.Contains(search) ||
                //                 a.Position.Contains(search) ||
                //                 a.Department.DepartmentName.Contains(search) 
                //         select a
                //                );
 

            var v = db.Employees.Include(e => e.Department);

            if (!String.IsNullOrEmpty(search))
            { 
                v = v.Where(s => s.FirstName.Contains(search) ||
                s.Position.Contains(search) || s.Department.DepartmentName.Contains(search));
            }
          

            totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,Position,DepartmentID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
               
              
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,Position,DepartmentID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // function ExportToExcel
        public void ExportToExcel()
        {
            var employees = db.Employees.Include(e => e.Department).ToList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            ws.Cells["A1"].Value = "Communication";
            ws.Cells["B1"].Value = "Com1";

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "report1";

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}",DateTimeOffset.Now) ;

            // start table 
            ws.Cells["A6"].Value = "EmployeeID";
            ws.Cells["B6"].Value = "FirstName";
            ws.Cells["C6"].Value = "LastName";
            ws.Cells["D6"].Value = "Position";
            ws.Cells["E6"].Value = "DepartmentName";

            int rowStart = 7;
            foreach (var item in employees)
            {
                //if (item.Experience < 5)
                //{
                //    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));

                //}

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EmployeeID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.FirstName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.LastName;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Position;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Department.DepartmentName;
                rowStart++;
            }
            // end table
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        
    }// end  ExportToExcel
     // Crystal Report
        public ActionResult Reports(string ReportType)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/EmployeeReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "EmployeeDataSet";
            reportDataSource.Value = db.Employees.ToList();
            localReport.DataSources.Add(reportDataSource);
            string mimeType;
            string encoding;
            string fileNameExtension;
            if (ReportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }
           else if (ReportType == "Word")
            {
                fileNameExtension = "docx";
            }
           else if (ReportType == "PDF")
            {
                fileNameExtension = "pdf";
            }
           else if (ReportType == "Image")
            {
                fileNameExtension = "jpg";
            }
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render(ReportType,"", out mimeType, out encoding , out fileNameExtension, out streams , out warnings);
            Response.AddHeader("content-disposition", "attachment: filename=" + "EmployeesReports."+fileNameExtension);
            return File(renderedByte,fileNameExtension);
        }// end crystal report

        // import file excel
        public ActionResult ImportExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
          //  if(Path.GetExtension(file.FileName)=="xlsx"|| Path.GetExtension(file.FileName) == "xls")
            //{
            //    ExcelPackage package = new ExcelPackage(file.InputStream);
            //    DataTable Dt = ExcelPackageExtensions.ToDataTable(package);
            //}
            return View();
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
