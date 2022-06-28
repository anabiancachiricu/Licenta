using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedOffice.Models
{
    public class Investigation
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestigationId { get; set; }
        [Required(ErrorMessage ="Numele este necesar")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Pretul este necesar")]
        public float Price { get; set; }
        [Required(ErrorMessage ="Este necesara selectarea unui departament")]
        public int DepartmentId { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public virtual Department Department { get; set; }

       // public virtual ICollection<Location> Locations { get; set; }
      

    }

}