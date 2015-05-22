using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class InvoiceSummary
    {
        public int InvoiceId { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal GstTotal { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public int SiteId { get; set; }
        public decimal NetworkChargesTotal { get; set; }
        public decimal EnergyChargesTotal { get; set; }
        public decimal MiscChargesTotal { get; set; }
        public decimal TotalCharges { get; set; }
        public decimal GSTCharges { get; set; }
        public decimal TotalNetworkCharges { get; set; }
        public decimal TotalMiscCharges { get; set; }
        public decimal TotalEnergyCharges { get; set; }
        public string ConnectionNumber { get; set; }
        public string SiteName { get; set; }
        public int EnergyPointId { get; set; }
        public int InvoiceSummaryId { get; set; }
    }
}
