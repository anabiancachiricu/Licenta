using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Investigation
    {
        [Key]
        public int InvestigationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public virtual Department Department { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<DoctorDetails> Doctors { get; set; }



    }

}