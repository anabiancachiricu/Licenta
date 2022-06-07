using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        [Required]
        public virtual DoctorDetails Doctor { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        
    }
}