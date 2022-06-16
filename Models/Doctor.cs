using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Models
{
    public class Doctor
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }
        public string UserId { get; set; }
        public int DepartmentId { get; set; }
        public int LocationId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Department Department { get; set; }
        public virtual Location Location { get; set; }

        


    }
}