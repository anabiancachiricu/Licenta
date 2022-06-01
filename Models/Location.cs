using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }

        public virtual ICollection<Department> Departments { get; set; }


    }
}