using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class InvoiceSummaryMap : EntityTypeConfiguration<InvoiceSummary>
    {
        public InvoiceSummaryMap()
        {
            // Primary Key
            this.HasKey(t => t.InvoiceSummaryId);

            // Properties
            this.Property(t => t.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AccountNumber)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.CustomerNumber)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.ConnectionNumber)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.SiteName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Month)
               .IsRequired()
               .HasMaxLength(10);

            this.Property(t => t.ApprovedById)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("InvoiceSummaries");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.InvoiceDate).HasColumnName("InvoiceDate");
            this.Property(t => t.InvoiceDueDate).HasColumnName("InvoiceDueDate");
            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");
            this.Property(t => t.GstTotal).HasColumnName("GstTotal");
            this.Property(t => t.InvoiceTotal).HasColumnName("InvoiceTotal");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.CustomerNumber).HasColumnName("CustomerNumber");
            this.Property(t => t.SiteId).HasColumnName("SiteId");
            this.Property(t => t.NetworkChargesTotal).HasColumnName("NetworkChargesTotal");
            this.Property(t => t.EnergyChargesTotal).HasColumnName("EnergyChargesTotal");
            this.Property(t => t.MiscChargesTotal).HasColumnName("MiscChargesTotal");
            this.Property(t => t.TotalCharges).HasColumnName("TotalCharges");
            this.Property(t => t.GSTCharges).HasColumnName("GSTCharges");
            this.Property(t => t.TotalNetworkCharges).HasColumnName("TotalNetworkCharges");
            this.Property(t => t.TotalMiscCharges).HasColumnName("TotalMiscCharges");
            this.Property(t => t.TotalEnergyCharges).HasColumnName("TotalEnergyCharges");
            this.Property(t => t.ConnectionNumber).HasColumnName("ConnectionNumber");
            this.Property(t => t.SiteName).HasColumnName("SiteName");
           // this.Property(t => t.EnergyPointId).HasColumnName("EnergyPointId");
            this.Property(t => t.InvoiceSummaryId).HasColumnName("InvoiceSummaryId");
            this.Property(t => t.ApprovedDate).HasColumnName("ApprovedDate");
            this.Property(t => t.EnergyPointId).HasColumnName("EnergyPointId");
           // this.Property(t => t.UserId).HasColumnName("ApprovedById");

            this.HasOptional(o => o.UserId);

            this.HasOptional(o => o.EnergyCharge)
                .WithRequired(ad => ad.InvoiceSummary).WillCascadeOnDelete(true);

            this.HasOptional(o => o.NetworkCharge)
                .WithRequired(ad => ad.InvoiceSummary).WillCascadeOnDelete(true);

            //this.HasOptional(o => o.EnergyPoint)
            //    .WithRequired(ad => ad.InvoiceSummary).WillCascadeOnDelete(true);

        }
    }
}
