﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CimscoPortal.Data
{
    using CimscoPortal.Infrastructure;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class CimscoPortalEntities : DbContext, ICimscoPortalEntities
    {
        public CimscoPortalEntities()
            : base("name=CimscoPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<MessageCategory> MessageCategories { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<PortalMessage> PortalMessages { get; set; }
        public virtual DbSet<InvoiceSummary> InvoiceSummaries { get; set; }
    }
}
