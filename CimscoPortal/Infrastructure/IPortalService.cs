using CimscoPortal.Data.Models;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
namespace CimscoPortal.Infrastructure
{
    public interface IPortalService
    {

        // Refactor status -->
        //CimscoPortal.Models.CustomerHierarchyViewModel GetCompanyHierarchy(string userId);
        CimscoPortal.Models.SiteHierarchyViewModel GetSiteHierarchy(string userId);
        CimscoPortal.Models.SummaryViewModel GetSummaryDataFor(string userId);
        IEnumerable<CimscoPortal.Models.InvoiceDetail> GetInvoiceDetailForSite(int contactId);
        CimscoPortal.Models.CommonInfoViewModel GetCommonData(string userId);
        CimscoPortal.Models.InvoiceDetailViewModel_ GetCurrentMonth(int _invoiceId);
        IEnumerable<CimscoPortal.Models.MessageViewModel> GetNavbarDataFor(string pageElement);
        InvoiceTallyViewModel GetInvoiceTally(string userId, int monthSpan);
        IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId);
        IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId, int invoiceId);
        InvoiceOverviewViewModel ApproveInvoice(int invoiceId, string userId);

        // <--

        //System.Collections.Generic.List<CimscoPortal.Models.AlertViewModel> GetAlertsFor(int category);

        CimscoPortal.Models.StackedBarChartViewModel GetHistoryByMonth(int _energyPointId);
        System.Collections.Generic.List<CimscoPortal.Models.AlertData> GetNavbarDataFor_Z(int customerId, string elementType);
      //  System.Data.Entity.DbSet<CimscoPortal.Data.PortalMessage> PortalMessages { get; }

        

        CimscoPortal.Models.InvoiceDetailViewModel GetCurrentMonth_(int _energyPointId);

        object ConfirmUserAccess(string p);

        

        System.Threading.Tasks.Task<List<CimscoPortal.Data.Models.AspNetUser>> GetUserByGroupOrCompany(string id);

        void UpdateUser(EditUserViewModel model);

        AspNetUser GetUserByID(string id);

        void UserloginUpdate(LoginHistory model);
    }

    public interface IMappingService
    {
        TDest Map<TSrc, TDest>(TSrc source) where TDest : class;
    }

    public class MappingService : IMappingService
    {
        public TDest Map<TSrc, TDest>(TSrc source) where TDest : class
        {
            return AutoMapper.Mapper.Map<TSrc, TDest>(source);
        }
    }

}
