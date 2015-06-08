using System;
namespace CimscoPortal.Infrastructure
{
    public interface IPortalService
    {
        //System.Collections.Generic.List<CimscoPortal.Models.AlertViewModel> GetAlertsFor(int category);
        CimscoPortal.Models.DonutChartViewModel GetCurrentMonth(int _energyPointId);
        CimscoPortal.Models.StackedBarChartViewModel GetHistoryByMonth(int _energyPointId);
        System.Collections.Generic.List<CimscoPortal.Models.AlertData> GetNavbarDataFor(int customerId, string elementType);
        System.Data.Entity.DbSet<CimscoPortal.Data.PortalMessage> PortalMessages { get; }

        System.Collections.Generic.IEnumerable<CimscoPortal.Models.MessageViewModel> GetNavbarDataForZ(int customerId, string pageElement);
        CimscoPortal.Models.CommonInfoViewModel GetCommonData();

        CimscoPortal.Models.CustomerHierarchyViewModel GetCompanyData(int contactId);
        System.Collections.Generic.IEnumerable<CimscoPortal.Models.InvoiceDetail> GetCompanyInvoiceData(int contactId);
        CimscoPortal.Models.SummaryViewModel GetSummaryDataFor(int customerId);

        CimscoPortal.Models.InvoiceDetailViewModel GetCurrentMonth_(int _energyPointId);
    }
}
