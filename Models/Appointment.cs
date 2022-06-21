using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Models
{
    public class Appointment
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> Doctors { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Doctor Doctor { get; set; }


    }
}