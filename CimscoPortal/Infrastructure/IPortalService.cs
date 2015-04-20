using System;
namespace CimscoPortal.Infrastructure
{
    public interface IPortalService
    {
        System.Collections.Generic.List<CimscoPortal.Models.AlertViewModel> GetAlertsFor(int category);
        CimscoPortal.Models.DonutChartViewModel GetCurrentMonth(int _energyPointId);
        System.Collections.Generic.List<CimscoPortal.Models.EnergyData> GetHistoryByMonth(int _energyPointId);
        System.Collections.Generic.List<CimscoPortal.Models.AlertViewModel> GetNavbarDataFor(int customerId, string elementType);
        System.Data.Entity.DbSet<CimscoPortal.Data.PortalMessage> PortalMessages { get; }
    }
}
