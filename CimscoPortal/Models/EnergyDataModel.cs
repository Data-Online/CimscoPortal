using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class EnergyDataModel
    {
        public List<decimal> EnergyRateByBracket { get; set; }
        public List<decimal> EnergyChargeByBracket { get; set; }
        public HeaderData HeaderData { get; set; }
        public decimal LossRate { get; set; }
        public decimal MaxCharge
        {
            get
            { return EnergyChargeByBracket.Max(); }
        }
        public decimal MaxRate
        {
            get { return EnergyRateByBracket.Max(); }
        }
        public decimal TotalCost
        {
            get { return EnergyChargeByBracket.Sum(); }
        }
        public decimal TotalLoss
        {
            get { return TotalCost * LossRate; }
        }
    }
}
