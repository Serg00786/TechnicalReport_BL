using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalReport_BL.Models
{
    public class Date
    {
        public int Eq_id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int DiscrId { get; set; }
        //public List<Discrepance> DiscrList { get; set; }

    }


    public class Discrepance
    {
        public int Id { get; set; }
        public string DiscrTime { get; set; }
    }


}
