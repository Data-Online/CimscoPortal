using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class EnergyDataModel
    {
        public List<decimal> EnergyCostByBracket { get; set; }
        public List<decimal> EnergyChargesByBracket { get; set; }
        public HeaderData HeaderData { get; set; }
        public decimal MaxCost
        {
            get { return EnergyCostByBracket.Max(); }
        }
        public decimal MaxCharge
        {
            get { return EnergyChargesByBracket.Max(); }
        }
        public decimal TotalCost
        {
            get { return EnergyCostByBracket.Sum(); }
        }
    }
}
