using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{
    public class Journal
    {
        public Journal()
        {
            Date = DateTime.Now.Date;
        }
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JournalId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Simptoms { get; set; }

        public DateTime? Date { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}