using CimscoPortal.Models;
using System;
using System.Collections.Generic;


namespace CimscoPortal.Infrastructure
{
    public interface IPortalService
    {
        System.Data.Entity.DbSet<CimscoPortal.Data.PortalMessage> PortalMessages { get; }

        List<AlertViewModel> GetAlertsFor(int category);

        List<AlertViewModel> GetNavbarDataFor(int customerId, string messageType);
    }

}
