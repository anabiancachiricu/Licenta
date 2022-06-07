using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class DoctorDetails
    {
        [Key]
        public int DoctorId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
       
        public virtual Department Department { get; set; }
        public virtual Location Location { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}