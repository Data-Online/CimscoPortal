﻿using CimscoPortal.Data.Models;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using CimscoPortal.Controllers.Api;

namespace CimscoPortal.Infrastructure
{
    public interface IPortalService
    {
        // Test data
        GoogleChartViewModel GetCostsAndConsumption(int monthSpan, CostConsumptionOptions options);

        InvoiceStatsViewModel GetDashboardStatistics(string userId, int monthSpan, string filter);

        // Refactor status -->
        //CimscoPortal.Models.CustomerHierarchyViewModel GetCompanyHierarchy(string userId);

        //DashboardViewData GetTotalCostsByMonth(string userId, int monthSpan, int? customerId);
        //DashboardViewData GetTotalCostsAndConsumption(string userId, int monthSpan, string filter);
        AvailableFiltersModel GetAllFilters(string userId);
        TextViewModel GetWelcomeScreen(string userId);
        SiteDetailViewModel GetSiteDetails(int siteId);

        CimscoPortal.Models.SiteHierarchyViewModel GetSiteHierarchy(string userId);
        CimscoPortal.Models.SummaryViewModel GetSummaryDataFor(string userId);
        IEnumerable<CimscoPortal.Models.InvoiceDetail> GetInvoiceDetailForSite(int contactId);
        CimscoPortal.Models.CommonInfoViewModel GetCommonData(string userId);
        CimscoPortal.Models.InvoiceDetailViewModel GetInvoiceDetail(int _invoiceId);
        IEnumerable<CimscoPortal.Models.MessageViewModel> GetNavbarDataFor(string pageElement);
        
        //InvoiceTallyViewModel GetInvoiceTally(string userId, int monthSpan, int? customerId);
        DetailBySiteViewModel GetDetailBySite(string userId, int monthSpan, string filter, int maximumSitesToReturn = 0);
        IEnumerable<InvoiceStatsBySiteViewModel> GetInvoiceStatsForSites(string userId, int monthSpan, string filter);

        // Invoice Data
        IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId);
        IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId, int monthsToDisplay);

        IEnumerable<InvoiceOverviewViewModel> GetAllInvoiceOverview(string userId, int monthSpan, string filter, int pageNo);

        InvoiceOverviewViewModel ApproveInvoice(int invoiceId, string userId, string urlRoot);
        bool SaveUserData(UserSettingsViewModel userSettings, string userId);
        IEnumerable<MonthlyConsumptionViewModal> GetHistoricalDataForSite(int siteId);
        UserAccessModel CheckUserAccess(string userName);
        UserSettingsViewModel GetUserSettings(string userName);
        // <--

        //System.Collections.Generic.List<CimscoPortal.Models.AlertViewModel> GetAlertsFor(int category);

        CimscoPortal.Models.StackedBarChartViewModel GetHistoryByMonth(int _energyPointId);
        System.Collections.Generic.List<CimscoPortal.Models.AlertData> GetNavbarDataFor_Z(int customerId, string elementType);
      //  System.Data.Entity.DbSet<CimscoPortal.Data.PortalMessage> PortalMessages { get; }

        

        //CimscoPortal.Models.InvoiceDetailViewModel GetCurrentMonth_(int _energyPointId);

        System.Threading.Tasks.Task<List<CimscoPortal.Data.Models.AspNetUser>> GetUserByGroupOrCompany(string id);

        void UpdateUser(EditUserViewModel model);

        AspNetUser GetUserByID(string id);

        void UserloginUpdate(LoginHistory model);

        bool LogFeedback(object data, string userId);
        DatapointDetailView GetDatapointDetails(DatapointIdentity datapointId);
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
