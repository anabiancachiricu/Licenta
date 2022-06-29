using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Location
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        [Required(ErrorMessage ="Orasul este necesar")]
        public string City { get; set; }
        [Required(ErrorMessage = "Adresa este necesara")]
        public string Address { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
      
        public virtual ICollection<Department> Departments { get; set; }


    }
}