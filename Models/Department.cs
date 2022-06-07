using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Department
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        [Required]
        public string DepartmentDescription { get; set; }

        //public virtual ICollection<Location> Locations { get; set; }
    }
}