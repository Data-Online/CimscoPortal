using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimscoPortal.Data.Models
{
    public partial class InvoiceSummary
    {

        public InvoiceSummary() { }

        public int InvoiceId { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public System.DateTime InvoiceDueDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal GstTotal { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public int SiteId { get; set; }
        public int EnergyPointId { get; set; }
        public decimal NetworkChargesTotal { get; set; }
        public decimal EnergyChargesTotal { get; set; }
        public decimal MiscChargesTotal { get; set; }
        public decimal TotalCharges { get; set; }
        public decimal GSTCharges { get; set; }
        public decimal TotalNetworkCharges { get; set; }
        public decimal TotalMiscCharges { get; set; }
        public decimal TotalEnergyCharges { get; set; }
        public decimal KwhTotal { get; set; }
        public string ConnectionNumber { get; set; }
        public string SiteName { get; set; }
       // public int EnergyPointId { get; set; }
        public bool Approved { get; set; }
        public bool Verified { get; set; }
        public int InvoiceSummaryId { get; set; }
        public string ApprovedById { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public decimal PercentageChange { get; set; }
        public System.DateTime PeriodStart { get; set; }
        public System.DateTime PeriodEnd { get; set; }
        public int SupplierId { get; set; }


        [ForeignKey("SupplierId")]
        public virtual EnergySupplier EnergySupplier { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        [ForeignKey("ApprovedById")]
        public virtual AspNetUser UserId { get; set; }

        public virtual EnergyCharge EnergyCharge { get; set; }
        public virtual NetworkCharge NetworkCharge { get; set; }
        public virtual OtherCharge OtherCharge { get; set; }

        [ForeignKey("EnergyPointId")]
        public virtual EnergyPoint EnergyPoint { get; set; }
    }
}
