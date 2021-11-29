using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalReport_BL.Models;

namespace TechnicalReport_BL.DTO
{
    public class DateDTO
    {
        public int Eq_id {get; set;}
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int DiscrId { get; set; }
    }
}
