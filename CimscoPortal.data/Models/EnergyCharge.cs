using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class EnergyCharge
    {

        [Key, ForeignKey("InvoiceSummary")]
        public int EnergyChargeId { get; set; }

        //public decimal LossRate { get; set; }
        // Business Day Charges by time slot
        public decimal BD0004 { get; set; }
        public decimal BD0408 { get; set; }
        public decimal BD0812 { get; set; }
        public decimal BD1216 { get; set; }
        public decimal BD1620 { get; set; }
        public decimal BD2024 { get; set; }

        // Non-Business Day Charges by time slot
        public decimal BD0004R { get; set; }
        public decimal BD0408R { get; set; }
        public decimal BD0812R { get; set; }
        public decimal BD1216R { get; set; }
        public decimal BD1620R { get; set; }
        public decimal BD2024R { get; set; }

        // Charge rates for above 
        public decimal NBD0004 { get; set; }
        public decimal NBD0408 { get; set; }
        public decimal NBD0812 { get; set; }
        public decimal NBD1216 { get; set; }
        public decimal NBD1620 { get; set; }
        public decimal NBD2024 { get; set; }

        public decimal NBD0004R { get; set; }
        public decimal NBD0408R { get; set; }
        public decimal NBD0812R { get; set; }
        public decimal NBD1216R { get; set; }
        public decimal NBD1620R { get; set; }
        public decimal NBD2024R { get; set; }

        public decimal BDQ0004 { get; set; }
        public decimal BDL0004 { get; set; }

        //// Service charges and rates
        //public decimal BDSVC { get; set; }
        //public decimal BDSVCR { get; set; }
        //public decimal NBDSVC { get; set; }
        //public decimal NBDSVCR { get; set; }

        //// Levy
        //public decimal EALevy { get; set; }
        //public decimal EALevyR { get; set; }

        // Calculated values
        public decimal LossRate
        { get { return (BDL0004 / BDQ0004); } }
        //{ get { return (BDL0004 / (BDQ0004 + BDL0004)); } }
        //public decimal BDMeteredKwh
        //{ get { return BD0004/BD0004R + BD0408/BD0004R + BD0812/BD0812R + BD1216/BD1216R + BD1620/BD1620R + BD2024/BD2024R; } }

        //public decimal BDLossCharge
        //{ get { return (BD0004 + BD0408 + BD0812 + BD1216 + BD1620 + BD2024) * LossRate; } }

        public virtual InvoiceSummary InvoiceSummary { get; set; }
    }
}
