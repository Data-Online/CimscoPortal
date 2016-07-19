﻿using CimscoPortal.Data;
using CimscoPortal.Infrastructure;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Data.Entity.SqlServer;
using System.Globalization;
using CimscoPortal.Data.Models;
using CimscoPortal.Extensions;

using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Web.Mvc;
using LinqKit;
using System.Linq.Expressions;

namespace CimscoPortal.Services
{
    class PortalService : IPortalService
    {
        ICimscoPortalContext _repository;

        private bool approved = true;
        private int MonthsOfHistoryData = 24; // GPA** Move to configuration for user

        private string _azurePDFsource = System.Configuration.ConfigurationManager.AppSettings["PdfFileSourceRoot"];

        public PortalService()
        {

        }
        public PortalService(ICimscoPortalContext repository)
        {
            this._repository = repository;
        }

        #region Common data and tools
        public UserAccessModel CheckUserAccess(string userName)
        {
            UserAccessModel _userAccess = new UserAccessModel();

            _userAccess.ValidSites = GetValidSiteIdListForUser(userName);
            _userAccess.CanApproveInvoices = CheckAuthorizedToApproveInvoice(userName);
            _userAccess.ViewInvoices = true;

            return _userAccess;
        }

        public CommonInfoViewModel GetCommonData(string userId)
        {
            //CommonInfoViewModel _commonData = new CommonInfoViewModel();
            CommonInfoViewModel _commonData = _repository.AspNetUsers.Where(s => s.UserName == userId).Project().To<CommonInfoViewModel>().FirstOrDefault();
            _commonData.TopLevelName = GetUserLevel(userId).TopLevelName;
            // Raise error if nothing returned - there should be available data for any logged in user
            if (_commonData == null)
            {
                LogMessage();
            }
            else
            {
                _commonData.UsefulInfo = new UsefulInfo { Temperature = "10", WeatherIcon = "wi wi-cloudy" };
            }
            return _commonData ?? new CommonInfoViewModel();
        }

        private static string GetSystemSettings(string setting)
        {
            return System.Configuration.ConfigurationManager.AppSettings[setting];
        }

        private void LogMessage()
        {

        }

        public IEnumerable<MessageViewModel> GetNavbarDataFor(string userName)
        {
            return _repository.PortalMessages.Where(i => i.User.Email == userName)
                                            .Project().To<MessageViewModel>();
        }

        public UserSettingsViewModel GetUserSettingsFor(string userName)
        {
            return new UserSettingsViewModel { MonthSpan = 12 };
        }

        public TextViewModel GetWelcomeScreen(string userId)
        {
            // Very basic - Update needed to make more flexable.
            var WelcomeScreenText = _repository.WelcomeScreen.Where(s => s.WelcomeId == 1).ToList()[0];
            TextViewModel WelcomeText = new TextViewModel();
            WelcomeText.Header = WelcomeScreenText.Initial;
            WelcomeText.Text = new List<string>();
            if (WelcomeScreenText.Line2.Trim().Length > 0)
                WelcomeText.Text.Add(WelcomeScreenText.Line2);
            if (WelcomeScreenText.Line3.Trim().Length > 0)
                WelcomeText.Text.Add(WelcomeScreenText.Line3);
            if (WelcomeScreenText.Line4.Trim().Length > 0)
                WelcomeText.Text.Add(WelcomeScreenText.Line4);
            if (WelcomeScreenText.Line5.Trim().Length > 0)
                WelcomeText.Text.Add(WelcomeScreenText.Line5);
            return WelcomeText;
        }
        #endregion

        #region Site data
        public SiteDetailViewModel GetSiteDetails(int siteId)
        {
            var _model = _repository.Sites.Where(s => s.SiteId == siteId).Project().To<SiteDetailViewModel>().FirstOrDefault();
            return _model;
        }

        #endregion Site data

        #region Dashboard

        public DashboardViewData GetTotalCostsAndConsumption(string userId, int monthSpan, string filter)
        {
            bool CreateTestData = false;
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

            DashboardViewData _model = new DashboardViewData();

            Dictionary<int, decimal> _monthTotals = new Dictionary<int, decimal>();
            Dictionary<int, int> _totalMissingInvoices = new Dictionary<int, int>();
            IQueryable<Site> _siteQuery = _repository.Sites;

            _siteQuery = ConstructSiteQueryFromFilter(filter, _siteQuery);
            IList<int> _allSitesInCurrentSelection = GetSiteIdListForUser(userId, _siteQuery);
            // Current date window
            DateTime _selectFromDate;
            DateTime _selectToDate;
            CalculateDateRange(monthSpan, out _selectFromDate, out _selectToDate);

            // Data for current (monthSpan) window
            IQueryable<InvoiceSummary> _invoiceData = ConstructInvoiceQueryForDates(_allSitesInCurrentSelection, _selectFromDate, _selectToDate);
            _model.InvoiceStats = CalculateStatisticsForFiledInvoices(monthSpan, _allSitesInCurrentSelection, _invoiceData);
            Dictionary<DateTime, MonthlySummaryModel> _invoiceTotals = GetDataSummary(_invoiceData, _selectFromDate, _selectToDate);

            // Data for prior -12 months
            _invoiceData = ConstructInvoiceQueryForDates(_allSitesInCurrentSelection, _selectFromDate.AddYears(-1), _selectToDate.AddYears(-1));
            Dictionary<DateTime, MonthlySummaryModel> _invoiceTotals12 = GetDataSummary(_invoiceData, _selectFromDate.AddYears(-1), _selectToDate.AddYears(-1));

            // Allocate data to view model and return this to the view
            ByMonthViewModel _costData = new ByMonthViewModel()
            {
                Months = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.MonthName).ToList(), // + ' ' + Get2LetterYear(t.Value.Year)).ToList(),
                Years = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.Year).ToList(),
                Values = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.InvoiceTotal).ToList(),
                Values12 = _invoiceTotals12.OrderBy(s => s.Key).Select(t => t.Value.InvoiceTotal).ToList(),
                TotalInvoices = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.TotalInvoices).ToList(),
                TotalInvoices12 = _invoiceTotals12.OrderBy(s => s.Key).Select(t => t.Value.TotalInvoices).ToList()
            };

            ByMonthViewModel _consumptionData = new ByMonthViewModel()
            {
                Months = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.MonthName).ToList(),// + ' ' + Get2LetterYear(t.Value.Year)).ToList(),
                Years = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.Year).ToList(),
                Values = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.EnergyTotal).ToList(),
                Values12 = _invoiceTotals12.OrderBy(s => s.Key).Select(t => t.Value.EnergyTotal).ToList(),
                TotalInvoices = _invoiceTotals.OrderBy(s => s.Key).Select(t => t.Value.TotalInvoices).ToList(),
                TotalInvoices12 = _invoiceTotals12.OrderBy(s => s.Key).Select(t => t.Value.TotalInvoices).ToList()
            };

            _model.Consumption = _consumptionData;
            _model.Cost = _costData;

            #region Create Test Data

            if (CreateTestData)
            {
                Random rnd = new Random();

                DateTime _firstDate = GetFirstDateForSelect(monthSpan);

                List<string> _months = new List<string>();
                List<int> _years = new List<int>();
                List<decimal> _values = new List<decimal>();
                List<decimal> _values12 = new List<decimal>();

                for (int _month = 1; _month <= monthSpan; _month++)
                {
                    _months.Add(_firstDate.ToString("MMMM"));
                    _years.Add(_firstDate.Year);
                    _values.Add(rnd.Next(3, 10) * 838.733M);
                    _values12.Add(rnd.Next(3, 10) * 1234.933M);
                    _firstDate = _firstDate.AddMonths(1);
                }

                _costData = new ByMonthViewModel()
                {
                    Months = _months,
                    Years = _years,
                    Values = _values,
                    Values12 = _values12
                };
            }
            #endregion

            return _model;
        }

        private IList<int> GetSiteIdListForUser(string userId, IQueryable<Site> query)
        {
            CurrentUserLevel _userLevel = GetUserLevel(userId);

            // Establish filters - if any
            //IQueryable<Site> _query = ConstructQueryFromPassedParameter(filter);

            // Select the data
            IList<int> _allSitesInCurrentSelection = AddQueryForGroupCustomerSite(_userLevel, query).Select(t => t.SiteId).ToList();
            return _allSitesInCurrentSelection;
        }

        public AvailableFiltersModel GetAllFilters(string userId)
        {
            CurrentUserLevel _userLevel = GetUserLevel(userId);
            AvailableFiltersModel _model = new AvailableFiltersModel();
            IQueryable<Site> _query = _repository.Sites;
            try
            {
                _model.Categories = AddQueryForGroupCustomerSite(_userLevel, _query).Select(t => t.IndustryClassification).Distinct().Project().To<FilterItem>().ToList();
                _model.Divisions = AddQueryForGroupCustomerSite(_userLevel, _query).Select(t => t.GroupDivision).Distinct().Project().To<FilterItem>().ToList();
            }
            catch { }
            return _model;
        }

        # region Dashboard data select private functions

        private static void CalculateDateRange(int monthSpan, out DateTime firstDate, out DateTime secondDate)
        {
            firstDate = GetFirstDateForSelect(monthSpan);
            secondDate = DateTime.Now.EndOfTheMonth();
        }

        private string Get2LetterYear(int fourLetterYear)
        {
            return fourLetterYear.ToString().Substring(2, 2);
        }

        private static InvoiceStatsViewModel CalculateStatisticsForFiledInvoices(int MonthSpan, IList<int> AllSitesInCurrentSelection, IQueryable<InvoiceSummary> InvoiceData)
        {
            var _data = new InvoiceStatsViewModel();
            var TotalPotenialSitesInPeriod = InvoiceData.Select(s => s.SiteId).Distinct().Count();
            var TotalPotentialInvoices = TotalPotenialSitesInPeriod * MonthSpan;
            var TotalInvoicesOnFileForPeriod = InvoiceData.Count();

            string _percentageMissingInvoices = (decimal.Round(
                NumericExtensions.SafeDivision((TotalInvoicesOnFileForPeriod * 100.0M), (TotalPotentialInvoices * 1.0M)), 0, MidpointRounding.AwayFromZero)).ToString();
            int _missingInvoices = TotalPotentialInvoices - TotalInvoicesOnFileForPeriod;
            string _percentageOfSitesWithDataOnFile = (decimal.Round(
                NumericExtensions.SafeDivision((TotalPotenialSitesInPeriod * 100.0M), (AllSitesInCurrentSelection.Count() * 1.0M)), 0, MidpointRounding.AwayFromZero)).ToString();
            _data.PercentMissingInvoices = _percentageMissingInvoices;
            _data.PercentSitesWithData = _percentageOfSitesWithDataOnFile;
            _data.TotalSites = AllSitesInCurrentSelection.Count();
            _data.TotalMissingInvoices = _missingInvoices;
            _data.TotalActiveSites = TotalPotenialSitesInPeriod;

            return _data;
        }

        private IQueryable<InvoiceSummary> ConstructInvoiceQueryForDates(IList<int> AllSitesInCurrentSelection, DateTime SelectFromDate, DateTime SelectToDate)
        {
            var _invoiceData = _repository.InvoiceSummaries.Where(s => AllSitesInCurrentSelection.Contains(s.SiteId) & s.PeriodEnd >= SelectFromDate & s.PeriodEnd <= SelectToDate);
            //            var InvoiceData = _repository.InvoiceSummaries.Where(s => AllSitesInCurrentSelection.Contains(s.SiteId) & s.InvoiceDate >= SelectFromDate & s.InvoiceDate <= SelectToDate);
            return _invoiceData;
        }

        private IQueryable<Site> AddQueryForGroupCustomerSite(CurrentUserLevel userLevel, IQueryable<Site> query)
        {
            switch (userLevel.UserLevel)
            {
                case "Group":
                    query = query.Where(s => s.Group.GroupName == userLevel.TopLevelName);
                    break;
                case "Customer":
                    query = query.Where(s => s.Customer.CustomerName == userLevel.TopLevelName);
                    break;
                case "Site":
                    query = query.Where(s => s.Customer.CustomerName == userLevel.TopLevelName);
                    break;
                default:
                    break;
            }
            return query;
        }


        #region Industry / Division filter construction
        private IQueryable<Site> ConstructSiteQueryFromFilter(string filter, IQueryable<Site> query)
        {
            // industry-industry.._division-division.. 
            //IQueryable<Site> _query = _repository.Sites;
            List<int> _industryClassificationIds;
            List<int> _divisionIds;
            try
            {
                string _passedIndustryClassificationIds = filter.Split('_')[1];
                string _passedDivisionIds = filter.Split('_')[2];
                _industryClassificationIds = _passedIndustryClassificationIds.IntegersFromString('-');
                _divisionIds = _passedDivisionIds.IntegersFromString('-');
            }
            catch { return query; }

            query = SearchByIndustryClassification(_industryClassificationIds, query);
            query = SearchByDivision(_divisionIds, query);
            return query;
        }

        private IQueryable<Site> SearchByIndustryClassification(List<int> keywords, IQueryable<Site> query)
        {
            //            IQueryable<Site> query = _repository.Sites;
            if (keywords.Count() > 0)
            {
                query = query.Where(p => keywords.Contains(p.IndustryClassification.IndustryId));
            }
            return query;
        }

        private IQueryable<Site> SearchByDivision(List<int> keywords, IQueryable<Site> query)
        {
            if (keywords.Count() > 0)
            {
                query = query.Where(p => keywords.Contains(p.GroupDivision.DivisionId));
            }
            return query;
        }
        #endregion

        private Dictionary<DateTime, MonthlySummaryModel> GetDataSummary(IQueryable<InvoiceSummary> InvoiceData, DateTime selectFromDate, DateTime selectToDate)
        {
            //var InvoiceData = _repository.InvoiceSummaries.Where(s => allSitesInCurrentSelection.Contains(s.SiteId) & s.InvoiceDate >= selectFromDate & s.InvoiceDate <= selectToDate);

            var _invoiceTotals = (from p in InvoiceData
                                  group p by new { p.InvoiceDate.Month, p.InvoiceDate.Year } into g
                                  select new MonthlySummaryModel
                                  {
                                      InvoiceTotal = (from f in g select f.InvoiceTotal).Sum(),
                                      EnergyTotal = (from f in g select f.KwhTotal).Sum(),
                                      TotalInvoices = (from f in g select f).Count(),
                                      InvoiceDate = (from f in g select f.InvoiceDate).FirstOrDefault()
                                  }).ToDictionary(k => k.KeyInvoiceDate);
            PopulateEmptyMonths(selectFromDate, selectToDate, _invoiceTotals);
            return _invoiceTotals;
        }

        private static void PopulateEmptyMonths(DateTime selectFromDate, DateTime selectToDate, Dictionary<DateTime, MonthlySummaryModel> _invoiceTotals)
        {
            DateTime _loopDate = selectFromDate;
            while (_loopDate <= selectToDate & _loopDate >= selectFromDate)
            {
                if (!_invoiceTotals.ContainsKey(_loopDate.StartOfThisMonth()))
                {
                    _invoiceTotals.Add(_loopDate.StartOfThisMonth(), new MonthlySummaryModel() { TotalInvoices = 0, InvoiceDate = _loopDate });
                }
                _loopDate = _loopDate.AddMonths(1);
            }
        }

        #endregion

        #endregion Dashboard data

        #region Site Overview data
        public GoogleChartViewModel GetCostsAndConsumption(string userId, int monthSpan, int siteId)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

            // siteId = 2;
            DateTime _selectFromDate;
            DateTime _selectToDate;
            CalculateDateRange(monthSpan, out _selectFromDate, out _selectToDate);

            IQueryable<Site> _siteQuery = _repository.Sites;
            IList<int> _allSitesInCurrentSelection = new List<int>();
            _allSitesInCurrentSelection.Add(siteId);

            IQueryable<InvoiceSummary> _invoiceData = ConstructInvoiceQueryForDates(_allSitesInCurrentSelection, _selectFromDate, _selectToDate).OrderBy(o => o.PeriodEnd);

            Dictionary<DateTime, MonthlySummaryModel> _result = CollateInvoiceData(_invoiceData);

            PopulateEmptyMonths(_selectFromDate, _selectToDate, _result);

            var _siteArea = _repository.Sites.Where(s => s.SiteId == siteId).Select(c => c.TotalFloorSpaceSqMeters).FirstOrDefault();
            GoogleChartViewModel _model = GoogleChartFor_PowerConsumption(_siteArea, _result);

            return _model;
        }



        public GoogleChartViewModel GetCostsAndConsumption(string userId, int monthSpan, string filter)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

            //string filter = "_827-_15-";

            // siteId = 2;
            DateTime _selectFromDate;
            DateTime _selectToDate;
            CalculateDateRange(monthSpan, out _selectFromDate, out _selectToDate);

            IQueryable<Site> _siteQuery = _repository.Sites;

            _siteQuery = ConstructSiteQueryFromFilter(filter, _siteQuery);

            IList<int> _allSitesInCurrentSelection = new List<int>();
            _allSitesInCurrentSelection = GetSiteIdListForUser(userId, _siteQuery);

            IQueryable<InvoiceSummary> _invoiceData = ConstructInvoiceQueryForDates(_allSitesInCurrentSelection, _selectFromDate, _selectToDate).OrderBy(o => o.PeriodEnd);

            Dictionary<DateTime, MonthlySummaryModel> _result = CollateInvoiceData(_invoiceData);

            PopulateEmptyMonths(_selectFromDate, _selectToDate, _result);

            var _siteArea = 0;
            GoogleChartViewModel _model = GoogleChartFor_PowerConsumption(_siteArea, _result);

            return _model;
        }

        private static Dictionary<DateTime, MonthlySummaryModel> CollateInvoiceData(IQueryable<InvoiceSummary> invoiceData)
        {
            var _result = (from p in invoiceData
                           group p by new { p.PeriodEnd.Month, p.PeriodEnd.Year } into g
                           select new MonthlySummaryModel()
                           {
                               InvoiceTotal = (from f in g select f.InvoiceTotal).Sum(),
                               EnergyTotal = (from f in g select f.KwhTotal).Sum(),
                               TotalInvoices = (from f in g select f).Count(),
                               InvoiceDate = (from f in g select f.InvoiceDate).FirstOrDefault()
                           }).ToDictionary(k => k.KeyInvoiceDate);
            return _result;
        }

        public GoogleChartViewModel GetCostsAndConsumption_orig(int siteId, int monthSpan)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

           // siteId = 2;

            DateTime _selectFromDate;
            DateTime _selectToDate;
            CalculateDateRange(monthSpan, out _selectFromDate, out _selectToDate);

            //var _query = _repository.InvoiceSummaries.Where(s => s.PeriodEnd > _selectFromDate).OrderBy(o => o.PeriodEnd);
            var _query = _repository.InvoiceSummaries.Where(s => s.SiteId == siteId && s.PeriodEnd > _selectFromDate).OrderBy(o => o.PeriodEnd);
            var _siteArea = _repository.Sites.Where(s => s.SiteId == siteId).Select(c => c.TotalFloorSpaceSqMeters).FirstOrDefault();

            var _result = (from p in _query
                           select new MonthlySummaryModel()
                           {
                               EnergyTotal = (decimal)p.KwhTotal,
                               InvoiceTotal = (decimal)p.InvoiceTotal - p.GstTotal,
                               InvoiceDate = p.PeriodEnd,
                               TotalInvoices = 1
                           }).ToDictionary(k => k.KeyInvoiceDate);
            PopulateEmptyMonths(_selectFromDate, _selectToDate, _result);

            GoogleChartViewModel _model = GoogleChartFor_PowerConsumption(_siteArea, _result);
            var zz = GetCostsAndConsumption(monthSpan);

            return zz;//_model;
        }

        public GoogleChartViewModel GetCostsAndConsumption(int monthSpan)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

            DateTime _selectFromDate;
            DateTime _selectToDate;
            CalculateDateRange(monthSpan, out _selectFromDate, out _selectToDate);

            var _query = _repository.InvoiceSummaries.Where(s => s.PeriodEnd > _selectFromDate).OrderBy(o => o.PeriodEnd);

            var _result = (from p in _query
                           group p by new { p.PeriodEnd.Month, p.PeriodEnd.Year } into g
                           select new MonthlySummaryModel()
                           {
                               InvoiceTotal = (from f in g select f.InvoiceTotal).Sum(),
                               EnergyTotal = (from f in g select f.KwhTotal).Sum(),
                               TotalInvoices = (from f in g select f).Count(),
                               InvoiceDate = (from f in g select f.InvoiceDate).FirstOrDefault()
                           }).ToDictionary(k => k.KeyInvoiceDate);
            PopulateEmptyMonths(_selectFromDate, _selectToDate, _result);

            GoogleChartViewModel _model = GoogleChartFor_PowerConsumption(1, _result);

            return _model;
        }

        #region Site Overview private functions
        //_new = new List<CPart>();
        //_new.Add(new CPart { v = "January", f = null });
        //_new.Add(new CPart { v = "19.12", f = "42 iteme" });
        //_new.Add(new CPart { v = "12", f = "Only 12 items" });
        //_new.Add(new CPart { v = "7", f = "7 servers" });
        //_new.Add(new CPart { v = "4", f = null });
        //_model.Rows.Add(new GoogleRows { Cparts = _new });
        //_new.Add(new CPart { v = "February", f = null });
        //_new.Add(new CPart { v = "13.32", f = null });
        //_new.Add(new CPart { v = "1", f = "1 unit left!" });
        //_new.Add(new CPart { v = "12", f = null });
        //_new.Add(new CPart { v = "2", f = null });
        //_model.Rows.Add(new GoogleRows { Cparts = _new });

        private static GoogleChartViewModel GoogleChartFor_PowerConsumption(int? SiteArea, Dictionary<DateTime, MonthlySummaryModel> Result)
        {
            GoogleChartViewModel _model = new GoogleChartViewModel();
            _model.Columns = new List<GoogleCols>();
            _model.Rows = new List<GoogleRows>();

            _model.Columns.Add(new GoogleCols { label = "Month", type = "string" });
            _model.Columns.Add(new GoogleCols { label = "Invoice Total excl GST", type = "number" });
            _model.Columns.Add(new GoogleCols { label = "Total Kwh", type = "number" });
            _model.Columns.Add(new GoogleCols { label = "Cost / Sqm", type = "number" });
            _model.Columns.Add(new GoogleCols { label = "Kwh / SqM", type = "number" });

            List<CPart> _dataRows;

            foreach (var _values in Result.OrderBy(o => o.Key))
            {
                string _currencySymbol = "$"; string _decimalFormat = "#,##0.00";
                string _invoiceTotal = _values.Value.InvoiceTotal == 0 ? null : _values.Value.InvoiceTotal.ToString();
                string _invoicePower = _values.Value.EnergyTotal == 0 ? null : _values.Value.EnergyTotal.ToString();
                string _invoiceTotalPerSqM = _values.Value.InvoiceTotal ==
                    0 ? null : NumericExtensions.SafeDivision(_values.Value.InvoiceTotal, (decimal)SiteArea).ToString();
                string _invoicePowerPerSqM = _values.Value.EnergyTotal ==
                    0 ? null : NumericExtensions.SafeDivision(_values.Value.EnergyTotal, (decimal)SiteArea).ToString();

                _dataRows = new List<CPart>();
                _dataRows.Add(new CPart { v = _values.Value.InvoiceDate.ToString("MMMM") + " '" + _values.Value.InvoiceDate.ToString("yy"), f = null });
                _dataRows.Add(new CPart { v = _invoiceTotal, f = _values.Value.InvoiceTotal.ToString(_currencySymbol + _decimalFormat) });
                _dataRows.Add(new CPart { v = _invoicePower, f = _values.Value.EnergyTotal.ToString(_decimalFormat) });
                _dataRows.Add(new CPart
                {
                    v = _invoiceTotalPerSqM,
                    f = NumericExtensions.SafeDivision(_values.Value.InvoiceTotal, (decimal)SiteArea).ToString(_currencySymbol + _decimalFormat)
                });
                _dataRows.Add(new CPart
                {
                    v = _invoicePowerPerSqM,
                    f = NumericExtensions.SafeDivision(_values.Value.EnergyTotal, (decimal)SiteArea).ToString(_decimalFormat)
                });
                _model.Rows.Add(new GoogleRows { Cparts = _dataRows });
            };
            return _model;
        }
        #endregion

        #endregion

        // GPA ** THIS NEEDS TO BE REMOVED
        public SiteHierarchyViewModel GetSiteHierarchy(string userId)
        {
            // Group level
            //      --> Customer
            //      --> Customer
            // Customer level
            //      --> Site
            //      --> Site
            // Site level

            SiteHierarchyViewModel _result = new SiteHierarchyViewModel();
            _result.UserLevel = GetUserCompanyOrGroup(userId);
            switch (_result.UserLevel)
            {
                case "Customer":
                    _result = _repository.Customers.Where(s => s.Users.Any(w => w.Email == userId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                    break;
                case "Group":
                    _result = _repository.Groups.Where(s => s.Users.Any(w => w.Email == userId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                    break;
                default:
                    // Raise error
                    break;
            }

            return _result;
        }


        public SiteHierarchyViewModel GetSiteHierarchy(string userId, bool customerDataOnly, int? customerId)
        {
            // Group level
            //      --> Site (owned by Company)
            //      --> Site (owned by Company)
            // Customer level
            //      --> Site
            //      --> Site
            // Site level

            SiteHierarchyViewModel _result = new SiteHierarchyViewModel();
            //string _userLevel = GetUserCompanyOrGroup(userId);
            //string[] _xz = GetUserCompanyOrGroupZ(userId);
            CurrentUserLevel _userLevel = GetUserLevel(userId);
            switch (_userLevel.UserLevel)
            {
                case "Customer":
                    _result = _repository.Customers.Where(s => s.Users.Any(w => w.Email == userId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                    break;
                case "Group":
                    int _firstOrDefaultCustomer = customerId ?? (int)_repository.Sites.Where(s => s.GroupId == _repository.Groups.Where(t => t.Users.Any(w => w.Email == userId))
                                                            .Select(w => w.GroupId).FirstOrDefault())
                                                            .Select(x => x.CustomerId).FirstOrDefault();
                    _result = _repository.Customers.Where(s => s.CustomerId == _firstOrDefaultCustomer).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                    _result.GroupName = _repository.Groups.Where(s => s.Users.Any(w => w.Email == userId)).FirstOrDefault().GroupName;
                    //_result = _repository.Groups.Where(s => s.Users.Any(w => w.Email == userId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                    break;
                case "Site":
                    //                    _result.TopLevelName = _userLevel.TopLevelName;
                    _result = _repository.Sites.Where(s => s.Users.Email == userId).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                    _result.SiteData = _repository.Sites.Where(s => s.Users.Email == userId).Project().To<SiteData>().Take(1).ToList();
                    break;
                default:
                    // Raise error
                    break;
            }
            _result.UserLevel = _userLevel.UserLevel;
            return _result;
        }


        public IEnumerable<InvoiceDetail> GetInvoiceDetailForSite(int siteId)
        {
            return InvoiceDetailForSite(siteId); //_invoices;
        }

        public IEnumerable<InvoiceDetail> GetInvoiceDetailForSite(int siteId, int invoiceId)
        {
            return InvoiceDetailForSite(siteId, invoiceId); //_invoices;
        }

        public IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId)
        {
            // SiteOverview
            return InvoiceOverviewForSite(siteId);
        }

        public IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId, int monthsToDisplay)
        {
            return InvoiceOverviewForSite(siteId, monthsToDisplay); //_invoices;
        }

        public IEnumerable<MonthlyConsumptionViewModal> GetHistoricalDataForSite(int invoiceId)
        {
            int _siteId = SiteIdFromInvoiceId(invoiceId);
            return HistoricalConsumption(_siteId, 12);
        }

        private int SiteIdFromInvoiceId(int invoiceId)
        {
            return _repository.InvoiceSummaries.Where(r => r.InvoiceId == invoiceId).Select(s => s.SiteId).FirstOrDefault();
        }

        private IEnumerable<InvoiceOverviewViewModel> InvoiceOverviewForSite(int siteId)
        {
            var _result = _repository.InvoiceSummaries.Where(s => s.SiteId == siteId)
                  .OrderByDescending(o => o.InvoiceDate).Project().To<InvoiceOverviewViewModel>().ToList();
            CheckPdfSourceFileExists(_result); // Only need to do this once? --> Move to Manage module
            // PopulateMissingInvoices(_result);

            return _result;
        }

        private IEnumerable<InvoiceOverviewViewModel> InvoiceOverviewForSite(int siteId, int monthsToDisplay)
        {
            DateTime _fromDate = DateTime.Now.AddMonths(Math.Max(monthsToDisplay, 1) * -1).StartOfThisMonth();
            var _result = _repository.InvoiceSummaries.Where(s => s.SiteId == siteId && s.InvoiceDate > _fromDate)
                  .OrderByDescending(o => o.InvoiceDate).Project().To<InvoiceOverviewViewModel>().ToList();
            CheckPdfSourceFileExists(_result); // Only need to do this once? --> Move to Manage module
            // PopulateMissingInvoices(_result);

            return _result;
        }

        // Rename this - updates data for a single invoice when approval triggered
        private InvoiceOverviewViewModel InvoiceOverviewForSite_(int invoiceId)
        {
            return _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId)
                  .OrderBy(o => o.InvoiceDate).Project().To<InvoiceOverviewViewModel>().FirstOrDefault();
        }

        private List<MonthlyConsumptionViewModal> HistoricalConsumption(int siteId, int months)
        {
            var _date = DateTime.Now.AddMonths(months * -1);
            // get consumption data for last year for site
            return _repository.InvoiceSummaries.Where(o => o.InvoiceDate > _date && o.SiteId == siteId).Project().To<MonthlyConsumptionViewModal>().ToList();
        }

        private void PopulateMissingInvoices(List<InvoiceOverviewViewModel> result)
        {
            //List<AddMonthData> _missingMonths = FindMissingMonths(_monthsToDisplay,
            //    AutoMapper.Mapper.Map<List<EnergyData>, List<AddMonthData>>(_dataListOrderedByInvoiceDate));

            var _missingInvoices = new List<DateTime>();

            DateTime _latestInvoiceDate = DateTime.Now.EndOfLastMonth();
            foreach (var _invoice in result)
            {
                if (!(_invoice.InvoiceDate.EndOfTheMonth() == _latestInvoiceDate))
                {
                    _missingInvoices.Add(_latestInvoiceDate);
                }
                _latestInvoiceDate = _latestInvoiceDate.EndOfLastMonth();
            }
            foreach (var _date in _missingInvoices)
            {
                var _data = new InvoiceOverviewViewModel() { InvoiceDate = _date, Missing = true };
                result.Add(_data);
            }
        }

        public SummaryViewModel GetSummaryDataFor(string userId)
        {
            decimal _maxInvoiceValue = 0.0M;
            SummaryViewModel _model = new SummaryViewModel();
            _model.SummaryData = new List<InvoiceDataForCompany>();

            int _invoiceHistoryMonths = MonthsOfHistoryData;

            List<InvoiceDetail> _invoicesDue;

            //var test = GetInvoiceHistory(24, 239, 29);
            SiteHierarchyViewModel _siteHierachy = GetSiteHierarchy(userId);
            InvoiceDataForCompany _invoiceDataForUser;
            int _index = 1;
            foreach (var _site in _siteHierachy.SiteData)
            {
                _invoicesDue = new List<InvoiceDetail>(GetInvoicesForSite(_site.SiteId, !approved));
                CheckInvoiceFileExists(_invoicesDue);
                _invoiceDataForUser = new InvoiceDataForCompany() { InvoiceHistory = GetInvoiceHistory(_invoiceHistoryMonths, _site.SiteId, _index), Year = 2014, InvoicesDue = _invoicesDue };
                // Year is not used at the moment, but may be helpful on graph?
                _model.SummaryData.Add(_invoiceDataForUser);
                _maxInvoiceValue = Math.Max(
                    Math.Max(
                        decimal.Parse(_invoiceDataForUser.InvoiceHistory.Max(s => s.YearA)),
                        decimal.Parse(_invoiceDataForUser.InvoiceHistory.Max(s => s.YearB))
                    ),
                    _maxInvoiceValue);
                _index++;
            }

            _model.SiteHierarchy = _siteHierachy;
            _model.MaxValue = Decimal.ToInt32(_maxInvoiceValue * 1.1M).RoundOff(); ;

            return _model;
        }

        public List<AlertData> GetNavbarDataFor_Z(int customerId, string pageElement)
        {
            var zz = _repository.PortalMessages.Where(i => i.CustomerId == 3).ToList();
            return _repository.PortalMessages.Where(i => i.MessageFormat.MessageType.PageElement == pageElement && i.CustomerId == customerId)
                                            .Project().To<AlertData>()
                                            .ToList();
        }

        public InvoiceDetailViewModel GetInvoiceDetail(int invoiceId)
        {
            // Read data from database
            InvoiceDetailViewModel _result = new InvoiceDetailViewModel();
            InvoiceDetail _invoiceDetail = GetInvoiceById(invoiceId);
            var _energyCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.EnergyCharge).FirstOrDefault();
            var _otherCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.OtherCharge).FirstOrDefault();
            var _networkCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.NetworkCharge).FirstOrDefault();

            // Normalise data for common presentation
            SetDefaultLossRateWhenZero(_invoiceDetail);
            SetDefaultEnergyChargesWhenFewerPeriods(_energyCharges);
            EnergyDataModel _businessDayData = AssignBusnessDayData(_energyCharges);
            EnergyDataModel _nonBusinessDayData = AssignNonBusinessDayData(_energyCharges);

            var _serviceCharges = new List<decimal> { _otherCharges.BDSVC, _otherCharges.BDSVCR };
            var _levyCharges = new List<decimal> { _otherCharges.EALevy, _otherCharges.EALevyR };

            List<EnergyDataModel> _energyDataModel = new List<EnergyDataModel>();
            _energyDataModel.Add(_businessDayData);
            _energyDataModel.Add(_nonBusinessDayData);

            // Assign data to the view
            _result.DonutChartData = AssignDataToDonutChart(_invoiceDetail); //_donutChartData;
            _result.InvoiceDetail = _invoiceDetail;
            _result.OtherCharges = AssignOtherChargesData(_otherCharges); ;// new List<decimal>() { _otherCharges.BDSVC, _otherCharges.NBDSVC, _otherCharges.EALevy, _invoiceDetail.MiscChargesTotal };
            _result.NetworkCharges = AssignNetworkChargesData(_networkCharges);// new List<decimal>() { _networkCharges.VariableBD, _networkCharges.VariableNBD, _networkCharges.CapacityCharge, _networkCharges.DemandCharge, _networkCharges.FixedCharge };
            //_result.InvoiceDetail.MiscChargesTotal = _invoiceDetail.EnergyChargesTotal - _businessDayData.TotalCost - _nonBusinessDayData.TotalCost + _invoiceDetail.MiscChargesTotal;
            // _result.InvoiceDetail.EnergyChargesTotal = _businessDayData.TotalCost + _nonBusinessDayData.TotalCost;
            _result.EnergyCosts = new EnergyCosts();
            _result.EnergyCosts.EnergyCostSeries = _energyDataModel;
            return _result;
        }

        private static List<DonutChartData> AssignDataToDonutChart(InvoiceDetail _invoiceDetail)
        {
            List<DonutChartData> _donutChartData = new List<DonutChartData> { 
                                    new DonutChartData() { Value = PercentToDecimal2(_invoiceDetail.EnergyChargesTotal, _invoiceDetail.InvoiceTotal), Label = _invoiceDetail.EnergyChargesLabel },
                                    new DonutChartData() { Value = PercentToDecimal2(_invoiceDetail.MiscChargesTotal,_invoiceDetail.InvoiceTotal) , Label = _invoiceDetail.MiscChargesLabel },
                                    new DonutChartData() { Value = PercentToDecimal2(_invoiceDetail.NetworkChargesTotal,_invoiceDetail.InvoiceTotal) , Label = _invoiceDetail.NetworkChargesLabel }
            };
            return _donutChartData;
        }

        private static List<decimal> AssignNetworkChargesData(NetworkCharge _networkCharges)
        {
            return new List<decimal>() { _networkCharges.VariableBD, _networkCharges.VariableNBD, _networkCharges.CapacityCharge, _networkCharges.DemandCharge, _networkCharges.FixedCharge };
        }

        private static List<decimal> AssignOtherChargesData(OtherCharge _otherCharges)
        {
            return new List<decimal>() { _otherCharges.BDSVC, _otherCharges.NBDSVC, _otherCharges.EALevy, _otherCharges.AdminCharge };
        }

        # region GetInvoiceDetail tools
        private static EnergyDataModel AssignNonBusinessDayData(EnergyCharge _energyCharges)
        {

            // REVISION ==>
            var zz = new List<ChartData>() { 
                new ChartData { Label = "Energy NBD0004", Value = _energyCharges.NBD0004 },
                new ChartData { Label = "Energy NBD0408", Value = _energyCharges.NBD0408 },
                new ChartData { Label = "Energy NBD0812", Value = _energyCharges.NBD0812 },
                new ChartData { Label = "Energy NBD1216", Value = _energyCharges.NBD1216 },
                new ChartData { Label = "Energy NBD1620", Value = _energyCharges.NBD1620 },
                new ChartData { Label = "Energy NBD2024", Value = _energyCharges.NBD2024 } 

            };

            var zzz = new List<ChartData>() { 
                new ChartData { Label = "Energy NBD0004R", Value = _energyCharges.NBD0004R },
                new ChartData { Label = "Energy NBD0408R", Value = _energyCharges.NBD0408R },
                new ChartData { Label = "Energy NBD0812R", Value = _energyCharges.NBD0812R },
                new ChartData { Label = "Energy NBD1216R", Value = _energyCharges.NBD1216R },
                new ChartData { Label = "Energy NBD1620R", Value = _energyCharges.NBD1620R },
                new ChartData { Label = "Energy NBD2024R", Value = _energyCharges.NBD2024R } 
            };

            EnergyDataModel _nonBusinessDayData = new EnergyDataModel()
            {
                EnergyChargeByBracket = new List<decimal> { _energyCharges.NBD0004, _energyCharges.NBD0408, _energyCharges.NBD0812, 
                                                            _energyCharges.NBD1216, _energyCharges.NBD1620, _energyCharges.NBD2024 },
                EnergyRateByBracket = new List<decimal> { _energyCharges.NBD0004R, _energyCharges.NBD0408R, _energyCharges.NBD0812R, 
                                                          _energyCharges.NBD1216R, _energyCharges.NBD1620R, _energyCharges.NBD2024R },
                HeaderData = new HeaderData() { Header = "Non Business Days" }
            };
            return _nonBusinessDayData;
        }

        private static EnergyDataModel AssignBusnessDayData(EnergyCharge _energyCharges)
        {
            EnergyDataModel _businessDayData = new EnergyDataModel()
            {
                EnergyChargeByBracket = new List<decimal> { _energyCharges.BD0004, _energyCharges.BD0408, _energyCharges.BD0812, 
                                                            _energyCharges.BD1216, _energyCharges.BD1620, _energyCharges.BD2024 },
                EnergyRateByBracket = new List<decimal>   { _energyCharges.BD0004R, _energyCharges.BD0408R, _energyCharges.BD0812R, 
                                                            _energyCharges.BD1216R, _energyCharges.BD1620R, _energyCharges.BD2024R },
                HeaderData = new HeaderData() { Header = "Business Days" }
            };
            return _businessDayData;
        }

        private void SetDefaultLossRateWhenZero(InvoiceDetail _invoiceDetail)
        {
            if (_invoiceDetail.LossRate == 0.0M) { _invoiceDetail.LossRate = Decimal.Parse(GetConfigValue("DefaultLossRate")); }
        }

        private static void SetDefaultEnergyChargesWhenFewerPeriods(EnergyCharge _energyCharges)
        {
            if (_energyCharges.BD0408 == 0.0M)
            {
                // 0004 --> 0408
                _energyCharges.BD0004 = (_energyCharges.BD0004 / 2.0M);
                _energyCharges.BD0408 = _energyCharges.BD0004;
                _energyCharges.BD0408R = _energyCharges.BD0004R;

                // 0812 --> 1216 --> etc
                _energyCharges.BD0812 = (_energyCharges.BD0812 / 4.0M);
                _energyCharges.BD1216 = _energyCharges.BD0812;
                _energyCharges.BD1216R = _energyCharges.BD0812R;
                _energyCharges.BD1620 = _energyCharges.BD0812;
                _energyCharges.BD1620R = _energyCharges.BD0812R;
                _energyCharges.BD2024 = _energyCharges.BD0812;
                _energyCharges.BD2024R = _energyCharges.BD0812R;

                _energyCharges.NBD0004 = (_energyCharges.NBD0004 / 2.0M);
                _energyCharges.NBD0408 = _energyCharges.NBD0004;
                _energyCharges.NBD0408R = _energyCharges.NBD0004R;

                _energyCharges.NBD0812 = (_energyCharges.NBD0812 / 4.0M);
                _energyCharges.NBD1216 = _energyCharges.NBD0812;
                _energyCharges.NBD1216R = _energyCharges.NBD0812R;
                _energyCharges.NBD1620 = _energyCharges.NBD0812;
                _energyCharges.NBD1620R = _energyCharges.NBD0812R;
                _energyCharges.NBD2024 = _energyCharges.NBD0812;
                _energyCharges.NBD2024R = _energyCharges.NBD0812R;
            }
        }
        #endregion

        //public InvoiceDetailViewModel GetCurrentMonth_(int _invoiceId)
        //{
        //    //string _dateFormat = "m";
        //    //GPA:  1. Region specific formats. 
        //    //      2. 
        //    //SiteHierarchyViewModel _siteHierarchy = _repository.Groups.Where(s => s.Users.Any(w => w.Id == _userRecordId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
        //    // InvoiceDetailViewModel _results = _repository.InvoiceSummaries.OrderByDescending(o => o.InvoiceDate).Where(r => r.InvoiceId == _invoiceId).Project().To<InvoiceDetailViewModel>().FirstOrDefault();
        //    var test = GetCurrentMonth(_invoiceId);
        //    IQueryable<DonutChartViewModel> q = from r in _repository.InvoiceSummaries.OrderByDescending(o => o.InvoiceDate)
        //                                        //where (r.EnergyPointId == _energyPointId)
        //                                        where (r.InvoiceId == _invoiceId)
        //                                        select new DonutChartViewModel()
        //                                        {
        //                                            DonutChartData = new List<DonutChartData> { 
        //                                                new DonutChartData() { Value = r.TotalEnergyCharges, Label = "Energy" },
        //                                                new DonutChartData() { Value = r.TotalMiscCharges, Label = "Other"},
        //                                                new DonutChartData() { Value = r.TotalNetworkCharges, Label = "Network"}
        //                                                },
        //                                            HeaderData = new HeaderData()
        //                                            {
        //                                                DataFor = "",
        //                                                Header = SqlFunctions.DateName("mm", r.InvoiceDate).ToUpper() + " " + SqlFunctions.DateName("YY", r.InvoiceDate).ToUpper()
        //                                            },
        //                                            SummaryData = new List<SummaryData> { new SummaryData() {   
        //                                                        Title = "BILL TOTAL", 
        //                                                        Detail = SqlFunctions.StringConvert(r.TotalEnergyCharges+r.TotalMiscCharges+r.TotalNetworkCharges) }, 
        //                                                      new SummaryData() { 
        //                                                        Title = "DUE DATE", 
        //                                                        Detail = SqlFunctions.DateName("dd", r.InvoiceDate) + " " +  SqlFunctions.DateName("mm", r.InvoiceDate) + " "  + SqlFunctions.DateName("YY", r.InvoiceDate)},
        //                                                      new SummaryData() { 
        //                                                        Title = "APPROVED BY", 
        //                                                        Detail = r.ApprovedById
        //                                                        },
        //                                                      new SummaryData() { 
        //                                                        Title = "APPROVAL DATE", 
        //                                                        Detail = r.ApprovedDate.ToString()
        //                                                        }
        //                                            },
        //                                            ApprovalData = new ApprovalData()
        //                                            {
        //                                                ApprovalDate = r.ApprovedDate,
        //                                                ApproverName = r.ApprovedById
        //                                            }
        //                                        };

        //    var _result = q.FirstOrDefault();
        //    _result.SummaryData[0].Detail = Convert.ToDecimal(_result.SummaryData[0].Detail).ToString("C");

        //    ///
        //    var model = ReturnTestEnergyDataModel();
        //    ////
        //    InvoiceDetailViewModel returnData = new InvoiceDetailViewModel();

        //    returnData.ChartData = _result;
        //    returnData.EnergyCostData = model;

        //    return returnData;

        //}

        public StackedBarChartViewModel GetHistoryByMonth(int _invoiceId)
        {
            int _monthsToDisplay = Convert.ToInt16(GetConfigValue("HistoryGraphMonths"));
            DateTime _fromMonth = DateTime.Today.AddMonths(_monthsToDisplay * -1);


            List<EnergyData> _dataListOrderedByInvoiceDate = EnergyDataForPeriod(_invoiceId, _fromMonth);
            //List<AddMonthData> zzzz = AutoMapper.Mapper.Map<List<EnergyData>, List<AddMonthData>>(_dataListOrderedByInvoiceDate);

            //List<AddMonthData> _missingMonths = FindMissingMonths(_monthsToDisplay, _fromMonth, _dataListOrderedByInvoiceDate);

            List<AddMonthData> _missingMonths = FindMissingMonths(_monthsToDisplay,
                AutoMapper.Mapper.Map<List<EnergyData>, List<AddMonthData>>(_dataListOrderedByInvoiceDate));


            EnergyData _emptyMonth = new EnergyData();
            foreach (var _newDate in _missingMonths)
            {
                _emptyMonth = new EnergyData();
                _emptyMonth.InvoiceDate = _newDate.InvoiceDate;
                _dataListOrderedByInvoiceDate.Insert(_newDate.monthCount, _emptyMonth);
            }

            if (_dataListOrderedByInvoiceDate.Last().TotalCharge > 0.0M)
            {
                _dataListOrderedByInvoiceDate.RemoveAt(0);
            }
            else
            {
                _dataListOrderedByInvoiceDate.RemoveAt(_monthsToDisplay);
            }

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.PercentDecimalDigits = 0;
            decimal _firstTotal = _dataListOrderedByInvoiceDate.First().TotalCharge;
            decimal _lastTotal = _dataListOrderedByInvoiceDate.Last().TotalCharge; //.ElementAtOrDefault(_dataListOrderedByInvoiceDate.Count() - _monthsToDisplay).TotalCharge;  // .Last().Energy;
            decimal _percentageChange = (1 - (NumericExtensions.SafeDivision(_firstTotal, _lastTotal)));

            BarChartSummaryData _summaryData = new BarChartSummaryData()
            {
                Title = "ELECTRICITY COSTS",
                SubTitle = "Invoice History. " +
                    _dataListOrderedByInvoiceDate.First().InvoiceDate.ToString("MMMM yyyy") +
                    " to " +
                    _dataListOrderedByInvoiceDate.Last().InvoiceDate.ToString("MMMM yyyy"),
                //_fromMonth.ToString("MMMM") + " " + _fromMonth.ToString("yyyy") + " to " +
                //DateTime.Today.ToString("MMMM") + " " + DateTime.Today.ToString("yyyy"),
                PercentChange = _percentageChange
            };

            var _result = new StackedBarChartViewModel();
            _result.BarChartSummaryData = _summaryData;
            _result.MonthlyData = _dataListOrderedByInvoiceDate;
            return _result;
        }

        private List<EnergyData> EnergyDataForPeriod(int _invoiceId, DateTime _fromMonth)
        {
            var _energyPointId = _repository.InvoiceSummaries.Where(s => s.InvoiceId == _invoiceId).FirstOrDefault().EnergyPoint.EnergyPointId;
            List<EnergyData> _dataListOrderedByInvoiceDate = _repository.InvoiceSummaries
                .Where(i => i.EnergyPoint.EnergyPointId == _energyPointId && i.InvoiceDate > _fromMonth)
                .OrderBy(o => o.InvoiceDate)
                .Project().To<EnergyData>()
                .ToList();
            return _dataListOrderedByInvoiceDate;
        }

        private static List<AddMonthData> FindMissingMonths(int _monthsToDisplay, DateTime _fromMonth, List<EnergyData> _dataListOrderedByInvoiceDate)
        {
            // Fill any blanks
            DateTime _expectThisDate;
            int _monthCount = 0;
            List<AddMonthData> _missingMonths = new List<AddMonthData>();

            foreach (var _nextDateFromInputList in _dataListOrderedByInvoiceDate)
            {
                _expectThisDate = _fromMonth.AddMonths(_monthCount);

                while (_expectThisDate.StartOfThisMonth() != _nextDateFromInputList.InvoiceDate.StartOfThisMonth())
                {
                    _missingMonths.Add(new AddMonthData { monthCount = _monthCount, InvoiceDate = _expectThisDate });
                    _monthCount++;
                    _expectThisDate = _fromMonth.AddMonths(_monthCount);
                }
                _monthCount++;
            }

            while (_monthCount <= _monthsToDisplay)
            {
                _missingMonths.Add(new AddMonthData { monthCount = _monthCount, InvoiceDate = _fromMonth.AddMonths(_monthCount) });
                _monthCount++;
            }
            return _missingMonths;
        }

        private static List<AddMonthData> FindMissingMonths(int _monthsToDisplay, List<AddMonthData> _dateList)
        {
            // Fill any blanks
            DateTime _expectThisDate;
            DateTime _fromMonth = DateTime.Today.AddMonths(_monthsToDisplay * -1);
            int _monthCount = 0;
            List<AddMonthData> _missingMonths = new List<AddMonthData>();

            foreach (var _nextDateFromInputList in _dateList)
            {
                _expectThisDate = _fromMonth.AddMonths(_monthCount);

                while (_expectThisDate.StartOfThisMonth() != _nextDateFromInputList.InvoiceDate.StartOfThisMonth())
                {
                    _missingMonths.Add(new AddMonthData { monthCount = _monthCount, InvoiceDate = _expectThisDate });
                    _monthCount++;
                    _expectThisDate = _fromMonth.AddMonths(_monthCount);
                }
                _monthCount++;
            }

            while (_monthCount <= _monthsToDisplay)
            {
                _missingMonths.Add(new AddMonthData { monthCount = _monthCount, InvoiceDate = _fromMonth.AddMonths(_monthCount) });
                _monthCount++;
            }
            return _missingMonths;
        }

        public void UpdateUser(EditUserViewModel model)
        {
            using (var db = new CimscoPortalContext())
            {
                var item = db.AspNetUsers.FirstOrDefault(f => f.Id == model.Id);
                item.Email = model.Email;
                item.FirstName = model.FirstName;
                item.LastName = model.LastName;
                item.PhoneNumber = model.Phone;
                db.AspNetUsers.Add(item);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public AspNetUser GetUserByID(string id)
        {
            using (var db = new CimscoPortalContext())
            {
                return db.AspNetUsers.FirstOrDefault(f => f.Id == id);
            }
        }

        public void UserloginUpdate(LoginHistory model)
        {
            using (var db = new CimscoPortalContext())
            {
                var item = db.LoginHistorys.FirstOrDefault(f => f.UserId == model.UserId);
                if (item != null)
                {
                    item.Details = model.Details;
                    item.Ip = model.Ip;
                    item.LastLoginDateTime = DateTime.Now;
                    item.UserId = model.UserId;
                    db.LoginHistorys.Add(item);
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    item = new LoginHistory();
                    item.Details = model.Details;
                    item.Ip = model.Ip;
                    item.LastLoginDateTime = DateTime.Now;
                    item.UserId = model.UserId;
                    db.LoginHistorys.Add(item);
                    db.Entry(item).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
        }

        public InvoiceTallyViewModel GetInvoiceTally(string userId, int monthSpan, int? customerId)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

            DateTime _firstDate = GetFirstDateForSelect(monthSpan);

            InvoiceTallyViewModel _invoiceTally = new InvoiceTallyViewModel();
            SiteHierarchyViewModel _siteListForUser = GetSiteHierarchy(userId, true, customerId); // GPA --> Pass last selected customer saved against account
            var _siteIdList = _siteListForUser.SiteData.Select(s => s.SiteId);
            var _siteData = _repository.InvoiceSummaries.Where(s => _siteIdList.Contains(s.SiteId) & s.InvoiceDate > _firstDate);

            if (_siteListForUser.UserLevel == "Group")
            {
                _invoiceTally.CustomerList = GetCustomerListForGroup((int)_siteListForUser.SiteData.Select(s => s.GroupId).FirstOrDefault());
                // var _customerListForGroup = GetCustomerListForGroup((int)_siteListForUser.SiteData.Select(s => s.GroupId).FirstOrDefault());
            }
            AutoMapper.Mapper.Map(_siteListForUser, _invoiceTally);
            _invoiceTally.InvoiceCosts = new List<InvoiceCosts>();

            CollateInvoiceDataBySiteId(_invoiceTally, _siteData, _firstDate);

            //_invoiceTally.GroupCompanyDetail.GroupCompanyName = _siteListForUser.GroupCompanyName;
            _invoiceTally.MonthsOfData = monthSpan;
            GetDetailBySite(userId, monthSpan);
            return _invoiceTally;
        }

        public DetailBySiteViewModel GetDetailBySite(string userId, int monthSpan)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; } // GPA --> move to constants

            DetailBySiteViewModel _model = new DetailBySiteViewModel();
            DateTime _selectFromDate = GetFirstDateForSelect(monthSpan);
            DateTime _selectToDate = DateTime.Now.EndOfTheMonth();

            // Select all sites for this user based on current filter (if any). Modify query if additional selection is required
            IQueryable<Site> _query = _repository.Sites;
            IList<int> _allSitesInCurrentSelection = GetSiteIdListForUser(userId, _query);

            var _selectedInvoices = _repository.InvoiceSummaries.Where(s => _allSitesInCurrentSelection.Contains(s.SiteId) & s.PeriodEnd >= _selectFromDate & s.PeriodEnd <= _selectToDate);
            var _siteQuery = _repository.Sites.Where(w => _allSitesInCurrentSelection.Contains(w.SiteId)).Distinct();
            List<SiteDetailData> _detailBySite = _siteQuery.Project().To<SiteDetailData>().ToList();
            _model.Divisions = _siteQuery.Select(s => s.GroupDivision.DivisionName).Distinct().ToList();
            _model.SiteDetailData = _detailBySite;

            CollateInvoiceDataBySiteId_(_model, _selectedInvoices, _selectFromDate);
            CreateEnergyChargeHistoryData(_model, _selectedInvoices);

            return _model;
        }

        private static void CreateEnergyChargeHistoryData(DetailBySiteViewModel model, IQueryable<InvoiceSummary> selectedInvoices)
        {
            var _energyChargeHistory = (from i in selectedInvoices
                                        //where i.SiteId == 5
                                        orderby i.InvoiceDate
                                        group i by i.SiteId into g
                                        select new
                                        {
                                            siteId = g.Key,
                                            month = (from l in g orderby l.InvoiceDate ascending select l.InvoiceDate),
                                            total = (from l in g orderby l.InvoiceDate ascending select l.EnergyChargesTotal) //l.InvoiceTotal - l.GSTCharges)
                                        });//.ToList();

            IEnumerable<decimal> _historyDataForSite;
            foreach (var _siteData in model.SiteDetailData)
            {
                try
                {
                    _historyDataForSite = _energyChargeHistory.Where(s => s.siteId == _siteData.SiteId).Select(s => s.total).ToList()[0];
                    _siteData.InvoiceHistory = new InvoiceHistory();
                    _siteData.InvoiceHistory.Totals = _historyDataForSite;
                }
                catch { }
            }
        }

        //
        private static void CollateInvoiceDataBySiteId_(DetailBySiteViewModel detailBySiteData, IQueryable<Data.Models.InvoiceSummary> siteData, DateTime firstDate)
        {
            var _approvedInvoiceCountBySiteId = (from p in siteData
                                                 where p.Approved == true
                                                 group p.Approved by p.SiteId into g
                                                 select new
                                                 {
                                                     siteId = g.Key,
                                                     count = (g.Count() > 0 ? g.Count() : 0)
                                                 }).ToList();

            var _invoioceCountAndDates = (from p in siteData
                                          group p by p.SiteId into g
                                          select new
                                          {
                                              siteId = g.Key,
                                              firstInvoiceOnFileDate = (from f in g orderby f.InvoiceDate ascending select f.InvoiceDate).FirstOrDefault(),
                                              latestInvoiceDate = (from l in g orderby l.InvoiceDate descending select l.InvoiceDate).FirstOrDefault(),
                                              count = (g.Count() > 0 ? g.Count() : 0)
                                          }).ToList();

            var _invoiceTotals = (from p in siteData
                                  group p by p.SiteId into g
                                  select new
                                  {
                                      siteId = g.Key,
                                      invoiceValue = (from f in g select f.InvoiceTotal - f.GstTotal).Sum(),
                                      energyCharge = (from f in g select f.EnergyChargesTotal).Sum(),
                                      // energyLosses = (from f in g select f.).Sum(),
                                      totalKwh = (from f in g select f.KwhTotal).Sum(),
                                      siteArea = (from f in g select f.Site.TotalFloorSpaceSqMeters).FirstOrDefault()
                                  }).ToList();

            var _calculatedLosses = (from p in siteData
                                     group p by p.SiteId into g
                                     select new
                                     {
                                         siteId = g.Key,
                                         calculatedLosses = (from f in g select f.EnergyCharge.BDL0004 / f.EnergyCharge.BDQ0004).FirstOrDefault()
                                     }).ToList();

            decimal _defaultLossRate = Convert.ToDecimal(GetSystemSettings("DefaultLossRate"));
            detailBySiteData.MaxTotalInvoices = _invoioceCountAndDates.Select(s => s.count).Max();
            decimal _maxEnergyCharge = _invoiceTotals.Select(s => s.energyCharge).Max();
            decimal _maxKwh = _invoiceTotals.Select(s => s.totalKwh).Max();

            foreach (var _entry in detailBySiteData.SiteDetailData)
            {
                var _matchingResultForApproved = _approvedInvoiceCountBySiteId.FirstOrDefault(s => s.siteId == _entry.SiteId);
                var _matchingResultForTotal = _invoioceCountAndDates.FirstOrDefault(s => s.siteId == _entry.SiteId);
                var _matchingResultForLosses = _calculatedLosses.FirstOrDefault(s => s.siteId == _entry.SiteId);
                var _match = detailBySiteData.SiteDetailData.Where(s => s.SiteId == _entry.SiteId).FirstOrDefault();
                _match.InvoiceKeyData = new InvoiceKeyData();
                _match.InvoiceCosts = new InvoiceCosts_();

                if (_matchingResultForTotal != null)
                {
                    if (_matchingResultForApproved != null)
                    {
                        _match.InvoiceKeyData.ApprovedInvoices = _matchingResultForApproved.count;
                    }
                    _match.InvoiceKeyData.PendingInvoices = _matchingResultForTotal.count - _match.InvoiceKeyData.ApprovedInvoices;
                    _match.InvoiceKeyData.FirstInvoiceDate = _matchingResultForTotal.firstInvoiceOnFileDate;
                    _match.InvoiceKeyData.LatestInvoiceDate = _matchingResultForTotal.latestInvoiceDate;
                    _match.InvoiceKeyData.CalculatedLossRate = Math.Round(_matchingResultForLosses.calculatedLosses, 3);
                    //_match.MissingInvoices = Math.Max(MonthsFromGivenDate(_matchingResultForTotal.firstInvoiceOnFileDate) - _match.TotalInvoicesOnFile, 0);
                };
                _match.InvoiceKeyData.MissingInvoices = Math.Max(MonthsFromGivenDate(firstDate) - _match.InvoiceKeyData.TotalInvoicesOnFile, 0);
                _match.InvoiceKeyData.CalculatedLossRate = Math.Max(_match.InvoiceKeyData.CalculatedLossRate, _defaultLossRate);

                var _matchingInvoiceData = _invoiceTotals.FirstOrDefault(s => s.siteId == _entry.SiteId);
                if (_matchingInvoiceData != null)
                {
                    _match.InvoiceCosts.InvoiceValue = _matchingInvoiceData.invoiceValue;
                    _match.InvoiceCosts.EnergyCharge = _matchingInvoiceData.energyCharge;
                    _match.InvoiceCosts.TotalKwh = _matchingInvoiceData.totalKwh;
                    _match.InvoiceCosts.SiteArea = _matchingInvoiceData.siteArea;
                    _match.InvoiceCosts.EnergyChargeByPercent = (_matchingInvoiceData.energyCharge / _maxEnergyCharge) * 100;
                    _match.InvoiceCosts.KwhByPercent = (_matchingInvoiceData.totalKwh / _maxKwh) * 100;
                }
            };

            decimal _maxCostPerSqM = detailBySiteData.SiteDetailData.Select(s => s.InvoiceCosts.CostPerSqm).Max();
            decimal _maxUnitsPerSqM = detailBySiteData.SiteDetailData.Select(s => s.InvoiceCosts.UnitsPerSqm).Max();

            foreach (var _entry in detailBySiteData.SiteDetailData)
            {
                _entry.InvoiceCosts.CostPerSqmByPercent = (_entry.InvoiceCosts.CostPerSqm / _maxCostPerSqM) * 100;
                _entry.InvoiceCosts.UnitsPerSqmByPercent = (_entry.InvoiceCosts.UnitsPerSqm / _maxUnitsPerSqM) * 100;
                
            }

        }



        //

        private List<CustomerHeader> GetCustomerListForGroup(int groupId)
        {
            var _result = (from p in _repository.Sites
                           where p.GroupId == groupId
                           select new CustomerHeader()
                                                {
                                                    CustomerId = p.CustomerId,
                                                    CustomerName = p.Customer.CustomerName
                                                }).Distinct().ToList();
            return _result;
        }

        private static DateTime GetFirstDateForSelect(int monthSpan)
        {
            DateTime _firstDate = DateTime.Now.AddMonths((monthSpan - 1) * -1);
            _firstDate = new DateTime(_firstDate.Year, _firstDate.Month, 1);
            return _firstDate;
        }



        #region update methods


        public InvoiceOverviewViewModel ApproveInvoice(int invoiceId, string userId, string rootUrl)
        {
            // Does user have authority to approve?
            // Should not get to this point, as button should not be available. But include test to be sure.

            UserAccessModel _access = CheckUserAccess(userId);
            InvoiceOverviewViewModel _result = new InvoiceOverviewViewModel();
            if (_access.CanApproveInvoices)
            {
                CimscoPortal.Data.Models.InvoiceSummary _invoice = new CimscoPortal.Data.Models.InvoiceSummary();
                try
                {
                    _invoice = _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId).FirstOrDefault();// .Find(invoiceId);
                    _invoice.Approved = true;
                    _invoice.ApprovedById = GetUserRecordId(userId);
                    _invoice.ApprovedDate = DateTime.Today;
                    //  _repository.Update
                    _repository.Update(_invoice);
                    _repository.Commit();

                    _result = InvoiceOverviewForSite_(invoiceId);
                }
                catch (Exception)
                {
                    return _result;
                    //throw;
                }

                // Approval users for site in question.
                SendApprovalMail(invoiceId, rootUrl);
            }
            return _result;
        }


        #endregion
        #region private methods

        private bool CheckAuthorizedToApproveInvoice(string userName)
        {
            //string[] stringSeparators = new string[] {";"};
            string[] _approvalRoles = GetConfigValue("CanApproveRoles")
                                        .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var _canApprove = _repository.AspNetUsers.Where(s => s.UserName == userName).FirstOrDefault()
                                            .AspNetRoles.Select(a => a.Name).ToList();

            foreach (string _role in _canApprove)
            {
                if (_canApprove.Contains(_role)) { return true; }
            }
            //& s.AspNetRoles.ToList().Contains('Approval')).FirstOrDefault();
            return false;
        }

        #region Email approval methods
        private bool SendApprovalMail(int invoiceId, string rootUrl)
        {
            // Create the email object first, then add the properties.
            string _sendGridApiKey = GetConfigValue("SendGridApi");
            string _sourceEmail = GetConfigValue("SourceEmail");
            string _logoImage = GetConfigValue("CimscoTextSml");
            string _rootForPdf = GetConfigValue("PdfFileSourceRoot");

            try
            {
                var myMessage = new SendGridMessage();

                // Add the message properties.
                myMessage.From = new MailAddress(_sourceEmail);
                List<String> _recipients = new List<string>();
                string _customerGroupName = "";
                string _siteName = "";
                _recipients = GetApprovalContactEmailsForInvoice(invoiceId, ref _customerGroupName, ref _siteName);
                myMessage.AddTo(_recipients);

                InvoiceDetail _invoiceDetail = new InvoiceDetail();
                _invoiceDetail = GetInvoiceById(invoiceId);

                myMessage.Subject = "Invoice " + _invoiceDetail.InvoiceNumber + " has been approved for payment";
                var _urlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
                var _linkToInvoiceDetail = "http://" + rootUrl + _urlHelper.Action("InvoiceDetail", "Portal", new { id = invoiceId });

                CreateApprovalMailMessage(myMessage, _invoiceDetail, _logoImage, _rootForPdf, _linkToInvoiceDetail,
                                            _customerGroupName, _siteName);

                // Create a Web transport, using API Key
                var transportWeb = new Web(_sendGridApiKey);

                // Send the email.
                transportWeb.DeliverAsync(myMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void CreateApprovalMailMessage(SendGridMessage myMessage, InvoiceDetail invoiceDetail, string logoImage,
            string rootForPdf, string linkToInvoiceDetail, string customerGroupName, string siteName)
        {
            System.Text.StringBuilder _sb;
            bool _formatAsText;
            int _columnPercentWidth = 50;

            // HTML

            _formatAsText = false;
            _sb = BuildMailMessage(invoiceDetail, logoImage, rootForPdf, linkToInvoiceDetail, _columnPercentWidth,
                customerGroupName, siteName, _formatAsText);
            myMessage.Html = _sb.ToString();

            // TEXT
            _formatAsText = true;
            _sb = BuildMailMessage(invoiceDetail, logoImage, rootForPdf, linkToInvoiceDetail, _columnPercentWidth,
                customerGroupName, siteName, _formatAsText);

            myMessage.Text = _sb.ToString();
        }

        #region Message construction methods
        private static StringBuilder BuildMailMessage(InvoiceDetail invoiceDetail, string logoImage, string rootForPdf, string linkToInvoiceDetail,
            int _columnPercentWidth, string customerGroupName, string siteName, bool formatAsText)
        {
            System.Text.StringBuilder sb;
            sb = new StringBuilder();
            MessageHeader(sb, formatAsText);
            MessageTitles(sb, customerGroupName, siteName, formatAsText);
            MessageLineEntries(invoiceDetail, sb, _columnPercentWidth, formatAsText);
            MessageLinks(sb, invoiceDetail, rootForPdf, linkToInvoiceDetail, formatAsText);
            MessageFooter(sb, logoImage, formatAsText);
            return sb;
        }

        private static void MessageFooter(System.Text.StringBuilder sb, string logoImage, bool text)
        {
            if (text)
            {
                sb.Append("\nwww.cimsco.co.nz");
            }
            else
            {
                sb.Append("<p><img alt='CimscoLogo'")
                        .Append("src='").Append(logoImage).Append("'></img>")
                        .Append("</br></p>");
                sb.Append("<p><a href=\"http://www.cimsco.co.nz\">www.cimsco.co.nz</a></p>");
                sb.Append("</body></html>");
            }
        }

        private static void MessageLinks(System.Text.StringBuilder sb, InvoiceDetail invoiceDetail, string rootForPdf, string linkToInvoiceDetail, bool text)
        {
            if (!text)
            {
                sb.Append("</br>");
                sb.Append("<p><a href=\"" + linkToInvoiceDetail + "\">Click here to view this invoice online</a></p>")
                    .Append("</br>");
                if (invoiceDetail.OnFile)
                {
                    sb.Append("<p><a href='")
                        .Append(ConstructInvoicePdfPath(invoiceDetail.SiteId, invoiceDetail.InvoiceId, rootForPdf))
                        .Append("'>Click here to download a copy of this invoice</a></p>");
                }
                else
                {
                    sb.Append("<p>Note: A scanned version of this invoice is not currently on file.</p>");
                }
                sb.Append("</br></br>");
            }
        }

        private static void MessageTitles(System.Text.StringBuilder sb, string customerGroupName, string siteName, bool text)
        {
            if (text)
            {
                sb.Append(customerGroupName + "\n");
                sb.Append("Site : " + siteName + "\n");
                sb.Append("\nInvoice Details\n");
            }
            else
            {
                sb.Append("<b>" + customerGroupName + "</b>").Append("<br/></br>");
                sb.Append("<b>Site : </b>" + siteName).Append("</br>");
                sb.Append("<p><b>Invoice Detals</b></p>").Append("<br/>");
            }
        }

        private static void MessageLineEntries(InvoiceDetail invoiceDetail, System.Text.StringBuilder sb, int columnWidth, bool text)
        {
            string _lineEntry;

            if (!text)
            {
                sb.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"500px\">");
            }
            _lineEntry = ConstructEmailLineEntry("Amount Due", string.Format(new CultureInfo("en-NZ", false), "{0:C}", invoiceDetail.InvoiceTotal), columnWidth, text);
            sb.Append(_lineEntry);
            _lineEntry = ConstructEmailLineEntry("Invoice Number", invoiceDetail.InvoiceNumber, columnWidth, text);
            sb.Append(_lineEntry);
            _lineEntry = ConstructEmailLineEntry("Invoice Date", invoiceDetail.InvoiceDate.ToLongDateString(), columnWidth, text);
            sb.Append(_lineEntry);
            _lineEntry = ConstructEmailLineEntry("Invoice Due Date", invoiceDetail.InvoiceDueDate.ToLongDateString(), columnWidth, text);
            sb.Append(_lineEntry);
            if (!text)
            {
                sb.Append("</table>");
            }
        }

        private static void MessageHeader(System.Text.StringBuilder sb, bool text)
        {
            if (!text)
            {
                sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><title>Invoice Approval</title>");
                sb.Append("<style type=\"text/css\">tab1 { padding-left: 4em; }</style>");
                sb.Append("<body>");
            }
        }

        private static string ConstructEmailLineEntry(string title, string value, int width, bool text)
        {
            string _style = "style='font-family:Helvetica, Arial, sans-serif;font-size:14px;color:#000;vertical-align:top;width:" + width + "%;padding-left:15px'";
            string _result;
            string _tabs = "\t\t";
            if (text)
            {
                if (title.Length > 15)
                { _tabs = "\t"; }
                _result = String.Format("\t{0}{2}{1}\n", title, value, _tabs);
            }
            else
            {
                _result = String.Format("<tr><td {2}>{0}</td><td {2}><i>{1}</i></td></tr>",
                                        title, value, _style);
            }
            return _result;
        }
        #endregion

        private List<string> GetApprovalContactEmailsForInvoice(int invoiceId, ref string customerGroupName, ref string siteName)
        {

            List<String> _recipients = new List<String>();

            try
            {
                RegexUtilities util = new RegexUtilities();
                var _canApproveList = Enumerable.Empty<string>()
                        .Select(r => new { Email = "", UserName = "" });

                int _siteId = _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId).Select(x => x.SiteId).FirstOrDefault();
                int? _groupId = _repository.Sites.Where(s => s.SiteId == _siteId).Select(x => x.GroupId).FirstOrDefault();
                int? _customerId = _repository.Sites.Where(s => s.SiteId == _siteId).Select(x => x.CustomerId).FirstOrDefault();
                siteName = _repository.Sites.Where(s => s.SiteId == _siteId).Select(x => x.SiteName).FirstOrDefault();

                if (_groupId != null)
                {
                    _canApproveList = (from role in _repository.AspNetRoles
                                       where role.Name == "Contact for Invoice Payment"
                                       from users in role.AspNetUsers
                                       from customers in users.Groups
                                       where customers.GroupId == _groupId
                                       select new { users.Email, users.UserName }).ToList();
                    customerGroupName = _repository.Groups.Where(s => s.GroupId == _groupId).Select(x => x.GroupName).FirstOrDefault();
                }
                else if (_customerId != null)
                {
                    _canApproveList = (from role in _repository.AspNetRoles
                                       where role.Name == "Contact for Invoice Payment"
                                       from users in role.AspNetUsers
                                       from customers in users.Customers
                                       where customers.CustomerId == _customerId
                                       select new { users.Email, users.UserName }).ToList();
                    customerGroupName = _repository.Customers.Where(s => s.CustomerId == _customerId).Select(x => x.CustomerName).FirstOrDefault();
                }
                else
                {
                    // No user name found
                }
                foreach (var _contact in _canApproveList)
                {
                    if (util.IsValidEmail((string)_contact.Email))
                    {
                        _recipients.Add((string)_contact.Email);
                    }
                    else if (util.IsValidEmail((string)_contact.UserName))
                    {
                        _recipients.Add((string)_contact.UserName);
                    }
                }
            }
            catch { }
#if DEBUG
            _recipients = new List<String>();

            string _testEmails = GetConfigValue("TestEmails");

            foreach (var _contact in _testEmails.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                _recipients.Add((string)_contact);
            }

#endif
            return _recipients;
        }
        #endregion

        private string GetConfigValue(string key)
        {
            return _repository.SystemConfiguration.Where(s => s.Key == key).Select(f => f.Values).FirstOrDefault();
        }

        private List<int> GetValidSiteIdListForUser(string userName)
        {
            return GetSiteHierarchy(userName, false, null).SiteData.Select(a => a.SiteId).ToList();
        }

        private static void CollateInvoiceDataBySiteId(InvoiceTallyViewModel invoiceTally, IQueryable<Data.Models.InvoiceSummary> siteData, DateTime firstDate)
        {
            var _approvedInvoiceCountBySiteId = (from p in siteData
                                                 where p.Approved == true
                                                 group p.Approved by p.SiteId into g
                                                 select new
                                                 {
                                                     siteId = g.Key,
                                                     count = (g.Count() > 0 ? g.Count() : 0)
                                                 }).ToList();

            var _invoioceCountAndDates = (from p in siteData
                                          group p by p.SiteId into g
                                          select new
                                          {
                                              siteId = g.Key,
                                              firstInvoiceOnFileDate = (from f in g orderby f.InvoiceDate ascending select f.InvoiceDate).FirstOrDefault(),
                                              latestInvoiceDate = (from l in g orderby l.InvoiceDate descending select l.InvoiceDate).FirstOrDefault(),
                                              count = (g.Count() > 0 ? g.Count() : 0)
                                          }).ToList();

            var _invoiceTotals = (from p in siteData
                                  group p by p.SiteId into g
                                  select new
                                  {
                                      siteId = g.Key,
                                      invoiceValue = (from f in g select f.InvoiceTotal).Sum(),
                                      energyCharge = (from f in g select f.EnergyChargesTotal).Sum(),
                                      // energyLosses = (from f in g select f.).Sum(),
                                      totalKwh = (from f in g select f.KwhTotal).Sum(),
                                      siteArea = (from f in g select f.Site.TotalFloorSpaceSqMeters).FirstOrDefault()
                                  }).ToList();

            var _calculatedLosses = (from p in siteData
                                     group p by p.SiteId into g
                                     select new
                                     {
                                         siteId = g.Key,
                                         calculatedLosses = (from f in g select f.EnergyCharge.BDL0004 / f.EnergyCharge.BDQ0004).FirstOrDefault()
                                     }).ToList();

            foreach (var _entry in invoiceTally.InvoiceTallies)
            {
                var _matchingResultForApproved = _approvedInvoiceCountBySiteId.FirstOrDefault(s => s.siteId == _entry.SiteId);
                var _matchingResultForTotal = _invoioceCountAndDates.FirstOrDefault(s => s.siteId == _entry.SiteId);
                var _matchingResultForLosses = _calculatedLosses.FirstOrDefault(s => s.siteId == _entry.SiteId);
                var _match = invoiceTally.InvoiceTallies.Where(s => s.SiteId == _entry.SiteId).FirstOrDefault();

                if (_matchingResultForTotal != null)
                {
                    if (_matchingResultForApproved != null)
                    {
                        _match.ApprovedInvoices = _matchingResultForApproved.count;
                    }
                    _match.PendingInvoices = _matchingResultForTotal.count - _match.ApprovedInvoices;
                    _match.FirstInvoiceDate = _matchingResultForTotal.firstInvoiceOnFileDate;
                    _match.LatestInvoiceDate = _matchingResultForTotal.latestInvoiceDate;
                    _match.CalculatedLossRate = Math.Round(_matchingResultForLosses.calculatedLosses, 3);
                    //_match.MissingInvoices = Math.Max(MonthsFromGivenDate(_matchingResultForTotal.firstInvoiceOnFileDate) - _match.TotalInvoicesOnFile, 0);
                };
                _match.MissingInvoices = Math.Max(MonthsFromGivenDate(firstDate) - _match.TotalInvoicesOnFile, 0);
                _match.CalculatedLossRate = Math.Max(_match.CalculatedLossRate, 0.028M);

                var _data = new InvoiceCosts();
                _data.SiteId = _entry.SiteId;
                var _matchingInvoiceData = _invoiceTotals.FirstOrDefault(s => s.siteId == _entry.SiteId);
                if (_matchingInvoiceData != null)
                {
                    _data.InvoiceValue = _matchingInvoiceData.invoiceValue;
                    _data.EnergyCharge = _matchingInvoiceData.energyCharge;
                    _data.TotalKwh = _matchingInvoiceData.totalKwh;
                    _data.SiteArea = _matchingInvoiceData.siteArea;
                }
                invoiceTally.InvoiceCosts.Add(_data);
            };
        }

        private static int MonthsFromGivenDate(DateTime date)
        {
            var _now = DateTime.Now;
            return ((_now.Year - date.Year) * 12) + _now.Month - date.Month;

            //int _years = _now.Year - date.Year;
            //int _monthsInPreviousYears = Math.Max((_years * 12) - date.Month + 1, 0);
            //int _monthsInThisYear = Math.Max(_now.Month, 0);
            //return (_monthsInPreviousYears + _monthsInThisYear);
        }


        private class CurrentUserLevel
        {
            public string UserLevel { get; set; }
            public string TopLevelName { get; set; }
        };

        // GPA --> duplicate
        private string GetUserCompanyOrGroup(string userId)
        {
            if (_repository.AspNetUsers.Where(s => s.Email == userId).FirstOrDefault().Groups.Any())
            {
                return "Group";
            }
            else if (_repository.AspNetUsers.Where(s => s.Email == userId).FirstOrDefault().Customers.Any())
            {
                return "Customer";
            }
            else
            {
                return "";
            }
        }

        private CurrentUserLevel GetUserLevel(string userId)
        {
            string _groupName = _repository.AspNetUsers.Where(s => s.Email == userId).FirstOrDefault().Groups.Select(g => g.GroupName).FirstOrDefault();
            string _customerName = _repository.AspNetUsers.Where(s => s.Email == userId).FirstOrDefault().Customers.Select(c => c.CustomerName).FirstOrDefault();


            if (!String.IsNullOrEmpty(_groupName))
            {
                return new CurrentUserLevel() { UserLevel = "Group", TopLevelName = _groupName };
            }
            else if (!String.IsNullOrEmpty(_customerName))
            {
                return new CurrentUserLevel() { UserLevel = "Customer", TopLevelName = _customerName };
            }
            else
            {
                string _siteName = _repository.AspNetUsers.Where(s => s.Email == userId).FirstOrDefault().Site.SiteName;
                return new CurrentUserLevel() { UserLevel = "Site", TopLevelName = _siteName };
            }
        }

        private List<InvoiceDetail> InvoiceDetailForSite(int siteId)
        {
            return _repository.InvoiceSummaries.Where(s => s.SiteId == siteId)
                .OrderBy(o => o.InvoiceDate).Project().To<InvoiceDetail>().ToList();
        }

        private List<InvoiceDetail> InvoiceDetailForSite(int siteId, int invoiceId)
        {
            return _repository.InvoiceSummaries.Where(s => s.SiteId == siteId & s.InvoiceId == invoiceId)
                .OrderBy(o => o.InvoiceDate).Project().To<InvoiceDetail>().ToList();
        }

        private List<InvoiceDetail> GetInvoicesForSite(int siteId, bool approved)
        {
            return _repository.InvoiceSummaries
                    .Where(s => s.SiteId == siteId && s.Approved == approved)
                    .OrderBy(o => o.InvoiceDate).Project().To<InvoiceDetail>().ToList();
        }

        private InvoiceDetail GetInvoiceById(int invoiceId)
        {
            var zz = _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId);
            return _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId).Project().To<InvoiceDetail>().FirstOrDefault();
        }

        private string GetUserRecordId(string userId)
        {
            var _userRecordId = _repository.AspNetUsers.Where(s => s.UserName == userId).Select(d => d.Id).FirstOrDefault();
            return _userRecordId;
        }

        public async Task<List<CimscoPortal.Data.Models.AspNetUser>> GetUserByGroupOrCompany(string id)
        {
            var _customers = _repository.Customers.FirstOrDefault(f => f.Users.Any(w => w.Id == id));
            if (_customers != null)
            {
                return await _repository.AspNetUsers.Where(w => w.Customers.Any(a => a.CustomerId == _customers.CustomerId)).ToListAsync();
            }
            else
            {
                return await _repository.AspNetUsers.Where(w => w.Groups.Any(f => f.GroupId == _repository.Groups.FirstOrDefault(g => g.Users.Any(a => a.Id == id)).GroupId)).ToListAsync();
            }
        }

        private static CommonInfoViewModel ProvideEmptyUserDetailsIfNull(CommonInfoViewModel _commonData)
        {
            if (_commonData == null)
            {
                _commonData = new CommonInfoViewModel { eMail = "" };
            };

            return _commonData;
        }

        private void CheckPdfSourceFileExists(List<InvoiceOverviewViewModel> invoiceList)
        {
            System.Net.HttpWebResponse response = null;
            System.Net.HttpWebRequest request;
            string _azurePDFsource = GetConfigValue("PdfFileSourceRoot");

            string _sourcePdf;
            foreach (var _invoice in invoiceList)
            {
                _sourcePdf = ConstructInvoicePdfPath(_invoice.SiteId, _invoice.InvoiceId, _azurePDFsource);
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_sourcePdf);
                request.Method = "Head";
                try
                {
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                }
                catch
                {
                    _invoice.InvoicePdf = false;
                }
                finally
                {
                    if (response != null)
                    {
                        _invoice.InvoicePdf = true;
                        response.Close();
                    }
                }
            }
        }

        private static string ConstructInvoicePdfPath(int siteId, int invoiceId, string azurePDFsource)
        {
            return azurePDFsource + "/" + siteId.ToString().PadLeft(6, '0') + "/" + invoiceId.ToString().PadLeft(8, '0') + ".pdf";
        }

        private void CheckInvoiceFileExists(List<InvoiceDetail> invoicesDue)
        {
            // This process needs to be moved to invoice manmagement module, settting "exists" flag there
            System.Net.HttpWebResponse response = null;
            System.Net.HttpWebRequest request;
            string _sourcePdf;
            foreach (var _invoice in invoicesDue)
            {
                _sourcePdf = _azurePDFsource + "/" + _invoice.SiteId.ToString().PadLeft(6, '0') + "/" + _invoice.InvoiceId.ToString().PadLeft(8, '0') + ".pdf";
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_sourcePdf);
                request.Method = "Head";
                try
                {
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                }
                catch
                {
                    _invoice.InvoicePdf = false;
                }
                finally
                {
                    if (response != null)
                    {
                        _invoice.InvoicePdf = true;
                        response.Close();
                    }
                }
            }
        }

        private List<CompanyInvoiceViewModel> GetInvoiceHistory(int spanMonths, int siteId, int index)
        {
            spanMonths = SetToEven(spanMonths);

            DateTime _today = DateTime.Today;
            DateTime _endOfCurrentMonth = LastDayOfMonth(_today);
            DateTime _startOfFirstPeriod = _endOfCurrentMonth.AddMonths(spanMonths * -1);
            DateTime _endOfFirstPeriod = _endOfCurrentMonth.AddMonths(spanMonths / 2 * -1);
            DateTime _startOfSecondPeriod = _endOfFirstPeriod;
            DateTime _endOfSecondPeriod = _endOfCurrentMonth;
            DateTime _currentDate = _startOfFirstPeriod.AddDays(1);

            var _dataForFirstPeriod = GetInvoiceDataFor(siteId, _startOfFirstPeriod, _endOfFirstPeriod);
            var _dataForSecondPeriod = GetInvoiceDataFor(siteId, _startOfSecondPeriod, _endOfSecondPeriod);

            List<CompanyInvoiceViewModel> _result = new List<CompanyInvoiceViewModel>();
            CompanyInvoiceViewModel _currentData;

            while (_currentDate < _startOfSecondPeriod)
            {
                int _monthNumber = _currentDate.Month;
                _currentData = new CompanyInvoiceViewModel();
                _currentData.Month = _currentDate.ToString("MMM");
                _currentData.SiteId = siteId;
                _currentData.Index = index;
                _currentData.YearA = _dataForFirstPeriod.Where(s => s.InvoiceDate.Month == _monthNumber).Select(s => s.InvoiceTotal).FirstOrDefault().ToString(); //a.InvoiceTotal.ToString();
                _currentData.YearB = _dataForSecondPeriod.Where(s => s.InvoiceDate.Month == _monthNumber).Select(s => s.InvoiceTotal).FirstOrDefault().ToString();
                _result.Add(_currentData);
                _currentDate = _currentDate.AddMonths(1);
            }

            return _result;
        }

        private static int SetToEven(int value)
        {
            if (value % 2 != 0) { value++; }
            return value;
        }

        private IQueryable<Data.Models.InvoiceSummary> GetInvoiceDataFor(int siteId, DateTime _startOfFirstPeriod, DateTime _endOfFirstPeriod)
        {
            var _dataForFirstPeriod = from a in _repository.InvoiceSummaries
                                      where a.InvoiceDate > _startOfFirstPeriod
                                              && a.InvoiceDate < _endOfFirstPeriod
                                              && a.SiteId == siteId
                                      select a;
            return _dataForFirstPeriod;
        }

        private static DateTime LastDayOfMonth(DateTime _today)
        {
            DateTime _endOfMonth = new DateTime(_today.Year, _today.Month, 1).AddMonths(1).AddDays(-1);
            return _endOfMonth;
        }

        private static decimal PercentToDecimal2(decimal value, decimal total)
        {
            return decimal.Round((value / total) * 100, 2, MidpointRounding.AwayFromZero);
        }

        private static List<EnergyDataModel> ReturnTestEnergyDataModel()
        {
            var model = new List<EnergyDataModel>() {   
                new EnergyDataModel { 
                    EnergyChargeByBracket = new List<decimal> { 10.665M, 11.756M, 15.639M, 14.786M, 16.199M, 13.918M }, 
                    EnergyRateByBracket = new List<decimal> { 239.14M, 923.52M, 2344.94M, 2041.24M, 2136.30M, 300.30M }, 
                    HeaderData = new HeaderData { Header = "Weekday Costs", DataFor = "1", _TempData = "WeeklyEnergyBySlice" }
                },
                new EnergyDataModel {
                    EnergyChargeByBracket = new List<decimal> { 8.888M, 9.797M, 13.032M, 12.319M, 13.499M, 11.599M }, 
                    EnergyRateByBracket = new List<decimal> { 82.38M, 249.15M, 1036.01M, 927.74M, 572.29M, 115.38M }, 
                    HeaderData = new HeaderData { Header = "Weekend Costs", DataFor = "2", _TempData = "WeekendEnergyBySlice" }
                }
            };
            return model;
        }


        #endregion
    }
}



//Random rnd = new Random()
//List<CompanyInvoiceViewModel> _invoiceData = new List<CompanyInvoiceViewModel>  {
//                                            new CompanyInvoiceViewModel { CustomerId=1,  Month = DateTime.Parse("02-01-2011").ToString("MMM"), YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Feb", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Mar", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Apr", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "May", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Jun", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "July", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Aug", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Sept", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Oct", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Nov", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Dec",YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() }
//};
//List<CompanyInvoiceViewModel> _invoiceData2 = new List<CompanyInvoiceViewModel>  {
//                                            new CompanyInvoiceViewModel { CustomerId=2,  Month = DateTime.Parse("02-01-2011").ToString("MMM"),YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Feb", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Mar", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Apr", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "May", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Jun", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "July", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Aug", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Sept", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Oct", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Nov", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
//                                            new CompanyInvoiceViewModel { CustomerId=2, Month = "Dec",YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() }
//};
