using System;
using System.Collections.Generic;
using System.Text;

namespace TechnicalReport_BL.Models
{
    public class FeModel
    {
        public Int64 UNIQUEID { get; set; }

        public int EQIDENT { get; set; }
        public int STATEIDENT { get; set; }

        public DateTime TS_START { get; set; }
        public DateTime TS_END { get; set; }
    }
}
