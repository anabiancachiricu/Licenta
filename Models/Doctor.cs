using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Department Department { get; set; }
        public virtual Location Location { get; set; }

        


    }
}