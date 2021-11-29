using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalReport_BL.Models.FE
{
    public class ProcessDowntimesModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Equipment")]
        public int Eq_id { get; set; }
        public string Machine { get; set; }
        [Required]
        [Display(Name = "Type of Downtime")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime from_dt { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime to_dt { get; set; }
    }
}
