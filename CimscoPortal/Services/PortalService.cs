using CimscoPortal.Data;
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

namespace CimscoPortal.Services
{
    class PortalService : IPortalService
    {
        ICimscoPortalContext _repository;

        private bool approved = true;

        public PortalService(ICimscoPortalContext repository)
        {
            this._repository = repository;
        }

        public object ConfirmUserAccess(string p)
        {
            return true;
        }

        public CommonInfoViewModel GetCommonData(string userId)
        {
            //userId = "user4@cimsco.co.nz";
            var CommonData = new CommonInfoViewModel();// { Temperature = "10", WeatherIcon = "wi wi-cloudy" };
            CommonData = _repository.AspNetUsers.Where(s => s.UserName == userId).Project().To<CommonInfoViewModel>().FirstOrDefault();
            //CommonData = new CommonInfoViewModel() {  FullName = "Test User", eMail = "test@test.com", CompanyLogo = "/Content/images/PakNSave.jpg" };
            CommonData.UsefulInfo = new UsefulInfo { Temperature = "10", WeatherIcon = "wi wi-cloudy" };
            return CommonData;
        }

        public IEnumerable<MessageViewModel> GetNavbarDataForZ(int customerId, string pageElement)
        {
            return _repository.PortalMessages.Where(i => i.MessageFormat.MessageType.PageElement == pageElement && i.CustomerId == customerId)
                                            .Project().To<MessageViewModel>()
                                            .ToList();
        }

        //public CustomerHierarchyViewModel GetCompanyHierarchy(string userId)
        //{
        //    // GPA : Allows for testing of API
        //    var _userRecordId = _repository.AspNetUsers.Where(s => s.UserName == userId).Select(d => d.Id).FirstOrDefault();
        //    // Hierachy from Group or Company to Site
        //    // Site has 1 or more energy points
        //    // Assume 1 invoice per energy point
        //    CustomerHierarchyViewModel _companyHierarchy = _repository.Groups.Where(i => i.Users.FirstOrDefault().Id == _userRecordId).Project().To<CustomerHierarchyViewModel>().FirstOrDefault();

        //    SiteHierarchyViewModel _siteHierarchy = _repository.Groups.Where(i => i.Users.FirstOrDefault().Id == _userRecordId).Project().To<SiteHierarchyViewModel>().FirstOrDefault();

        //    return _companyHierarchy;
        //}

        public SiteHierarchyViewModel GetSiteHierarchy(string userId)
        {
            // Group level
            // Customer level
            // Site level
            var _userRecordId = GetUserRecordId(userId);
            //var test = _repository.Customers.Where(s => s.Users.Any(w => w.Id == _userRecordId));
            SiteHierarchyViewModel _siteHierarchy = _repository.Groups.Where(s => s.Users.Any(w => w.Id == _userRecordId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
            if (_siteHierarchy == null)
                _siteHierarchy = _repository.Customers.Where(s => s.Users.Any(w => w.Id == _userRecordId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();

            return _siteHierarchy;
        }



        private List<InvoiceDetail> GetInvoicesForSite(int siteId, bool approved)
        {
            List<InvoiceDetail> _invoicesForApproval = _repository.InvoiceSummaries.Where(s => s.SiteId == siteId && s.Approved == approved).Project().To<InvoiceDetail>().ToList();
            return _invoicesForApproval;
        }

        private InvoiceDetail GetInvoiceById(int invoiceId)
        {
            var zz = _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId);
            return _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId).Project().To<InvoiceDetail>().FirstOrDefault();
        }

        public IEnumerable<InvoiceDetail> GetSiteInvoiceData(int siteId)
        {
            List<InvoiceDetail> _invoices = GetInvoicesForSite(siteId, approved);
            //List<InvoiceDetail2> _invoices = new List<InvoiceDetail2>();
            //if (contactId == 1)
            //{
            //    _invoices.Add(new InvoiceDetail2 { Amount = 10352, DueDate = new DateTime(2014, 1, 1), PercentChange = 2, InvoiceId = 1 });
            //    _invoices.Add(new InvoiceDetail2 { Amount = 14362, DueDate = new DateTime(2014, 2, 1), PercentChange = 4, InvoiceId = 2 });
            //}
            //else
            //{
            //    _invoices.Add(new InvoiceDetail2 { Amount = 13944, DueDate = new DateTime(2014, 1, 1), PercentChange = 3, InvoiceId = 3 });
            //    _invoices.Add(new InvoiceDetail2 { Amount = 19833, DueDate = new DateTime(2014, 2, 1), PercentChange = 4, InvoiceId = 4 });
            //}
            return _invoices;
        }

        public SummaryViewModel GetSummaryDataFor(string userId)
        {

            //var test = _repository.InvoiceSummaries.Where(x => x.SiteId == 231).Project().To<CompanyInvoiceViewModel2>().ToList();
            SummaryViewModel model = new SummaryViewModel();
            model.SummaryData = new List<InvoiceDataForCompany>();
            int _invoiceHistoryMonths = 24;

            List<InvoiceDetail> _invoicesDue;

            //var test = GetInvoiceHistory(24, 239, 29);
            SiteHierarchyViewModel _siteHierachy = GetSiteHierarchy(userId);
            InvoiceDataForCompany _invoiceHistory;
            int _index = 1;
            foreach (var s in _siteHierachy.SiteData)
            {
                _invoicesDue = new List<InvoiceDetail>(GetInvoicesForSite(s.SiteId, !approved));
                _invoiceHistory = new InvoiceDataForCompany() { InvoiceHistory = GetInvoiceHistory(_invoiceHistoryMonths, s.SiteId, _index), Year = 2014, InvoicesDue = _invoicesDue };
                model.SummaryData.Add(_invoiceHistory);
                _index++;
            }

            model.SiteHierarchy = _siteHierachy;

            model.MaxValue = 30000;

            return model;
        }

        private List<CompanyInvoiceViewModel2> GetInvoiceHistory(int spanMonths, int siteId, int index)
        {
            DateTime _today = DateTime.Today;
            DateTime _endOfMonth = new DateTime(_today.Year, _today.Month, 1).AddMonths(1).AddDays(-1);
            DateTime _startDateA = new DateTime(_today.Year, _today.Month, 1).AddMonths(1).AddDays(-1).AddMonths(spanMonths * -1);
            DateTime _endDateA = new DateTime(_today.Year, _today.Month, 1).AddMonths(1).AddDays(-1).AddMonths(spanMonths / 2 * -1);
            DateTime _startDateB = _endDateA;
            DateTime _endDateB = _endOfMonth;
            DateTime _currentDate = _startDateA.AddDays(1);

            string startMonth = DateTime.Today.AddMonths(spanMonths * -1).ToString("MMMM");
            var _yearA = from a in _repository.InvoiceSummaries
                         where a.InvoiceDate > _startDateA
                                 && a.InvoiceDate < _endDateA
                                 && a.SiteId == siteId
                         select a;

            var _yearB = from a in _repository.InvoiceSummaries
                         where a.InvoiceDate > _startDateB
                                 && a.InvoiceDate < _endDateB
                                 && a.SiteId == siteId
                         select a;

            List<CompanyInvoiceViewModel2> _result = new List<CompanyInvoiceViewModel2>();
            CompanyInvoiceViewModel2 _currentData;

            _endOfMonth = _endOfMonth.AddMonths(spanMonths / 2 * -1);
            while (_currentDate < _endOfMonth)
            {
                int _monthNumber = _currentDate.Month;
                _currentData = new CompanyInvoiceViewModel2();
                _currentData.Month = _currentDate.ToString("MMM");
                _currentData.SiteId = siteId;
                _currentData.Index = index;
                _currentData.YearA = _yearA.Where(s => s.InvoiceDate.Month == _monthNumber).Select(s => s.InvoiceTotal).FirstOrDefault().ToString(); //a.InvoiceTotal.ToString();
                _currentData.YearB = _yearB.Where(s => s.InvoiceDate.Month == _monthNumber).Select(s => s.InvoiceTotal).FirstOrDefault().ToString();
                _result.Add(_currentData);
                _currentDate = _currentDate.AddMonths(1);
            }

            return _result;
        }


        public List<AlertData> GetNavbarDataFor(int customerId, string pageElement)
        {
            var zz = _repository.PortalMessages.Where(i => i.CustomerId == 3).ToList();
            return _repository.PortalMessages.Where(i => i.MessageFormat.MessageType.PageElement == pageElement && i.CustomerId == customerId)
                                            .Project().To<AlertData>()
                                            .ToList();
        }

        public InvoiceDetailViewModel_ GetCurrentMonth(int invoiceId)
        {
            InvoiceDetailViewModel_ _result = new InvoiceDetailViewModel_();
            InvoiceDetail _invoiceDetail = GetInvoiceById(invoiceId);

            _invoiceDetail.ValidationError = (_invoiceDetail.InvoiceTotal != _invoiceDetail.EnergyChargesTotal + _invoiceDetail.MiscChargesTotal + _invoiceDetail.NetworkChargesTotal);


            //_result.EnergyCosts.EnergyCostSeries = ReturnTestEnergyDataModel();
            _result.InvoiceDetail = _invoiceDetail;
            var _energyCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.EnergyCharge).FirstOrDefault();
            var _networkCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.NetworkCharge).FirstOrDefault();
           // var _energyCharges = _energyCharges.FirstOrDefault();//.Select(a => a.BD0004 + a.BD0408).FirstOrDefault();
            List<EnergyDataModel> _energyDataModel = new List<EnergyDataModel>();
            EnergyDataModel _businessDayData = new EnergyDataModel()
            {
                EnergyChargeByBracket = new List<decimal> { _energyCharges.BD0004, _energyCharges.BD0408, _energyCharges.BD0812, _energyCharges.BD1216, _energyCharges.BD1620, _energyCharges.BD2024 },
                EnergyRateByBracket = new List<decimal> { _energyCharges.BD0004R, _energyCharges.BD0408R, _energyCharges.BD0812R, _energyCharges.BD1216R, _energyCharges.BD1620R, _energyCharges.BD2024R },
                HeaderData = new HeaderData() {  Header = "Business Days"}
            };

            EnergyDataModel _nonBusinessDayData = new EnergyDataModel()
            {
                EnergyChargeByBracket = new List<decimal> { _energyCharges.NBD0004, _energyCharges.NBD0408, _energyCharges.NBD0812, _energyCharges.NBD1216, _energyCharges.NBD1620, _energyCharges.NBD2024 },
                EnergyRateByBracket = new List<decimal> { _energyCharges.NBD0004R, _energyCharges.NBD0408R, _energyCharges.NBD0812R, _energyCharges.NBD1216R, _energyCharges.NBD1620R, _energyCharges.NBD2024R },
                HeaderData = new HeaderData() {  Header = "Non Business Days"}
            };

            _result.OtherCharges = new List<decimal>() { _energyCharges.BDSVC, _energyCharges.NBDSVC, _energyCharges.EALevy, _invoiceDetail.MiscChargesTotal };
            _result.NetworkCharges = new List<decimal>() { _networkCharges.VariableBD, _networkCharges.VariableNBD, _networkCharges.CapacityCharge, _networkCharges.DemandCharge, _networkCharges.FixedCharge };

            var _zzMiscCharges = _result.InvoiceDetail.MiscChargesTotal;
            _result.InvoiceDetail.MiscChargesTotal = _result.InvoiceDetail.EnergyChargesTotal - _businessDayData.TotalCost - _nonBusinessDayData.TotalCost + _zzMiscCharges;
            _result.InvoiceDetail.EnergyChargesTotal = _businessDayData.TotalCost + _nonBusinessDayData.TotalCost;

            var _serviceCharges = new List<decimal> { _energyCharges.BDSVC, _energyCharges.BDSVCR };
            var _levyCharges = new List<decimal> {_energyCharges.EALevy, _energyCharges.EALevyR };
           // var _summaryData = new List<decimal> { _energyCharges.LossRate, _energyCharges.BDMeteredKwh, _energyCharges.BDLossCharge };

            List<DonutChartData> _donutChartData = new List<DonutChartData> { 
                                    new DonutChartData() { Value = PercentToDecimal2(_invoiceDetail.EnergyChargesTotal, _invoiceDetail.InvoiceTotal), Label = "Energy" },
                                    new DonutChartData() { Value = PercentToDecimal2(_invoiceDetail.MiscChargesTotal,_invoiceDetail.InvoiceTotal) , Label = "Other" },
                                    new DonutChartData() { Value = PercentToDecimal2(_invoiceDetail.NetworkChargesTotal,_invoiceDetail.InvoiceTotal) , Label = "Network" }
            };


            _result.DonutChartData = _donutChartData;

            _energyDataModel.Add(_businessDayData);
            _energyDataModel.Add(_nonBusinessDayData);

            _result.EnergyCosts = new EnergyCosts();
            _result.EnergyCosts.EnergyCostSeries = _energyDataModel;

            return _result;
        }

        private static decimal PercentToDecimal2(decimal value, decimal total)
        {
            return decimal.Round((value / total)*100, 2, MidpointRounding.AwayFromZero);
        }

        public InvoiceDetailViewModel GetCurrentMonth_(int _invoiceId)
        {
            //string _dateFormat = "m";
            //GPA:  1. Region specific formats. 
            //      2. 
            //SiteHierarchyViewModel _siteHierarchy = _repository.Groups.Where(s => s.Users.Any(w => w.Id == _userRecordId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
            // InvoiceDetailViewModel _results = _repository.InvoiceSummaries.OrderByDescending(o => o.InvoiceDate).Where(r => r.InvoiceId == _invoiceId).Project().To<InvoiceDetailViewModel>().FirstOrDefault();
            var test = GetCurrentMonth(_invoiceId);
            IQueryable<DonutChartViewModel> q = from r in _repository.InvoiceSummaries.OrderByDescending(o => o.InvoiceDate)
                                                //where (r.EnergyPointId == _energyPointId)
                                                where (r.InvoiceId == _invoiceId)
                                                select new DonutChartViewModel()
                                                {
                                                    DonutChartData = new List<DonutChartData> { 
                                                        new DonutChartData() { Value = r.TotalEnergyCharges, Label = "Energy" },
                                                        new DonutChartData() { Value = r.TotalMiscCharges, Label = "Other"},
                                                        new DonutChartData() { Value = r.TotalNetworkCharges, Label = "Network"}
                                                        },
                                                    HeaderData = new HeaderData()
                                                    {
                                                        DataFor = "",
                                                        Header = SqlFunctions.DateName("mm", r.InvoiceDate).ToUpper() + " " + SqlFunctions.DateName("YY", r.InvoiceDate).ToUpper()
                                                    },
                                                    SummaryData = new List<SummaryData> { new SummaryData() {   
                                                                Title = "BILL TOTAL", 
                                                                Detail = SqlFunctions.StringConvert(r.TotalEnergyCharges+r.TotalMiscCharges+r.TotalNetworkCharges) }, 
                                                              new SummaryData() { 
                                                                Title = "DUE DATE", 
                                                                Detail = SqlFunctions.DateName("dd", r.InvoiceDate) + " " +  SqlFunctions.DateName("mm", r.InvoiceDate) + " "  + SqlFunctions.DateName("YY", r.InvoiceDate)},
                                                              new SummaryData() { 
                                                                Title = "APPROVED BY", 
                                                                Detail = r.ApprovedById
                                                                },
                                                              new SummaryData() { 
                                                                Title = "APPROVAL DATE", 
                                                                Detail = r.ApprovedDate.ToString()
                                                                }
                                                    },
                                                    ApprovalData = new ApprovalData() { 
                                                        ApprovalDate =  r.ApprovedDate, 
                                                        ApproverName = r.ApprovedById 
                                                    }
                                                };

            var _result = q.FirstOrDefault();
            _result.SummaryData[0].Detail = Convert.ToDecimal(_result.SummaryData[0].Detail).ToString("C");

            ///
            var model = ReturnTestEnergyDataModel();
            ////
            InvoiceDetailViewModel returnData = new InvoiceDetailViewModel();

            returnData.ChartData = _result;
            returnData.EnergyCostData = model;

            return returnData;

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

        public StackedBarChartViewModel GetHistoryByMonth(int _invoiceId)
        {
            var _energyPointId = _repository.InvoiceSummaries.Where(s => s.InvoiceId == _invoiceId).FirstOrDefault().EnergyPoint.EnergyPointId;
            var _result = new StackedBarChartViewModel();
            DateTime _fromMonth = DateTime.Today.AddMonths(-13);
            List<EnergyData> _data = _repository.InvoiceSummaries.Where(i => i.EnergyPoint.EnergyPointId == _energyPointId && i.InvoiceDate > _fromMonth).OrderBy(o => o.InvoiceDate)
                                            .Project().To<EnergyData>()
                                            .ToList();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.PercentDecimalDigits = 0;
            decimal zz = _data.First().Energy;
            decimal zzz = _data.ElementAtOrDefault(_data.Count() - 13).Energy;  // .Last().Energy;
            decimal _percentageChange = (1 - (zz / zzz)); //.ToString("P", nfi);
            BarChartSummaryData _summaryData = new BarChartSummaryData()
            {
                Title = "ELECTRICITY COSTS",
                SubTitle = "Invoice History. " +
                    _fromMonth.ToString("MMMM") + " " + _fromMonth.ToString("yyyy") + " to " +
                    DateTime.Today.ToString("MMMM") + " " + DateTime.Today.ToString("yyyy"),
                PercentChange = _percentageChange
            };

            _result.BarChartSummaryData = _summaryData;
            _data.RemoveAt(12);
            _result.MonthlyData = _data;

            for (int i = 0; i < _result.MonthlyData.Count(); i++)
            {
                _result.MonthlyData[i].Month = _result.MonthlyData[i]._month.ToString("MMM");
            }
            return _result;
            //return new StackedBarChartViewModel();
        }

        #region update methods

        public void ApproveInvoice(int invoiceId, string userId)
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
            }
            catch (Exception)
            {

                throw;
            }


        }

        #endregion
        #region private methods
        private string GetUserRecordId(string userId)
        {
            var _userRecordId = _repository.AspNetUsers.Where(s => s.UserName == userId).Select(d => d.Id).FirstOrDefault();
            return _userRecordId;
        }
        #endregion
    }
}



//Random rnd = new Random();
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
