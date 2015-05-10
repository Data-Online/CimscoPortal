using System;
using CimscoPortal.Data;

namespace CimscoPortal.Infrastructure
{
    interface ICimscoPortalEntities
    {
        System.Data.Entity.DbSet<Customer> Customers { get; set; }
        System.Data.Entity.DbSet<MessageType> MessageTypes { get; set; }
        System.Data.Entity.DbSet<PortalMessage> PortalMessages { get; set; }
        System.Data.Entity.DbSet<InvoiceSummary> InvoiceSummaries { get; set; }
    }
}
