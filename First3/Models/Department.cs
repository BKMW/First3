using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace First3.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }




        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        [NotMapped]
        [Display(Name ="Image")]
        public string ImageUrl { get {
                return DepartmentID.ToString() + ".jpg";
            }
            set { } }
    }
}