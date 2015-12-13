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
using CimscoPortal.Data.Models;
using CimscoPortal.Extensions;

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

        public object ConfirmUserAccess(string p)
        {
            return true;
        }

        public CommonInfoViewModel GetCommonData(string userId)
        {
            CommonInfoViewModel _commonData = new CommonInfoViewModel();
            _commonData = _repository.AspNetUsers.Where(s => s.UserName == userId).Project().To<CommonInfoViewModel>().FirstOrDefault();
            // Raise error if nothing returned - there should be available data for any logged in user

            _commonData.UsefulInfo = new UsefulInfo { Temperature = "10", WeatherIcon = "wi wi-cloudy" };
            return _commonData;
        }

        public IEnumerable<MessageViewModel> GetNavbarDataFor(string userName)
        {
            return _repository.PortalMessages.Where(i => i.User.Email == userName)
                                            .Project().To<MessageViewModel>();
        }

        public SiteHierarchyViewModel GetSiteHierarchy(string userId)
        {
            // Group level
            // Customer level
            // Site level
            switch (GetUserCompanyOrGroup(userId))
            {
                case "Customer":
                    return _repository.Customers.Where(s => s.Users.Any(w => w.Email == userId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                case "Group":
                    return _repository.Groups.Where(s => s.Users.Any(w => w.Email == userId)).Project().To<SiteHierarchyViewModel>().FirstOrDefault();
                default:
                    // Raise error
                    return new SiteHierarchyViewModel();
            }
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
            return InvoiceOverviewForSite(siteId); //_invoices;
        }

        public IEnumerable<InvoiceOverviewViewModel> GetInvoiceOverviewForSite(int siteId, int invoiceId)
        {
            return InvoiceOverviewForSite(siteId, invoiceId); //_invoices;
        }

        private IEnumerable<InvoiceOverviewViewModel> InvoiceOverviewForSite(int siteId)
        {
            return _repository.InvoiceSummaries.Where(s => s.SiteId == siteId)
                  .OrderBy(o => o.InvoiceDate).Project().To<InvoiceOverviewViewModel>().ToList();
        }

        private IEnumerable<InvoiceOverviewViewModel> InvoiceOverviewForSite(int siteId, int invoiceId)
        {
            return _repository.InvoiceSummaries.Where(s => s.SiteId == siteId & s.InvoiceId == invoiceId)
                  .OrderBy(o => o.InvoiceDate).Project().To<InvoiceOverviewViewModel>().ToList();
        }

        private InvoiceOverviewViewModel InvoiceOverviewForSite_(int invoiceId)
        {
            return _repository.InvoiceSummaries.Where(s => s.InvoiceId == invoiceId)
                  .OrderBy(o => o.InvoiceDate).Project().To<InvoiceOverviewViewModel>().FirstOrDefault();
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
                checkInvoiceFileExists(_invoicesDue);
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

        public InvoiceDetailViewModel_ GetCurrentMonth(int invoiceId)
        {
            InvoiceDetailViewModel_ _result = new InvoiceDetailViewModel_();
            InvoiceDetail _invoiceDetail = GetInvoiceById(invoiceId);

            _invoiceDetail.ValidationError = (_invoiceDetail.InvoiceTotal != _invoiceDetail.EnergyChargesTotal + _invoiceDetail.MiscChargesTotal + _invoiceDetail.NetworkChargesTotal);

            if (_invoiceDetail.LossRate == 0.0M) { _invoiceDetail.LossRate = 0.028M; }
            //_result.EnergyCosts.EnergyCostSeries = ReturnTestEnergyDataModel();
            _result.InvoiceDetail = _invoiceDetail;
            var _energyCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.EnergyCharge).FirstOrDefault();
            var _otherCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.OtherCharge).FirstOrDefault();
            if (_energyCharges.BD0408 == 0.0M)
            {
                // 0004 --> 0408
                _energyCharges.BD0004 = (_energyCharges.BD0004 / 2.0M) + (_invoiceDetail.LossRate * (_energyCharges.BD0004 / 2.0M));
                _energyCharges.BD0408 = _energyCharges.BD0004;
                _energyCharges.BD0408R = _energyCharges.BD0004R;

                // 0812 --> 1216 --> etc
                _energyCharges.BD0812 = (_energyCharges.BD0812 / 4.0M) + (_invoiceDetail.LossRate * (_energyCharges.BD0812 / 4.0M));
                _energyCharges.BD1216 = _energyCharges.BD0812;
                _energyCharges.BD1216R = _energyCharges.BD0812R;
                _energyCharges.BD1620 = _energyCharges.BD0812;
                _energyCharges.BD1620R = _energyCharges.BD0812R;
                _energyCharges.BD2024 = _energyCharges.BD0812;
                _energyCharges.BD2024R = _energyCharges.BD0812R;

                _energyCharges.NBD0004 = (_energyCharges.NBD0004 / 2.0M) + (_invoiceDetail.LossRate * (_energyCharges.NBD0004 / 2.0M));
                _energyCharges.NBD0408 = _energyCharges.NBD0004;
                _energyCharges.NBD0408R = _energyCharges.NBD0004R;

                _energyCharges.NBD0812 = (_energyCharges.NBD0812 / 4.0M) + (_invoiceDetail.LossRate * (_energyCharges.NBD0812 / 4.0M));
                _energyCharges.NBD1216 = _energyCharges.NBD0812;
                _energyCharges.NBD1216R = _energyCharges.NBD0812R;
                _energyCharges.NBD1620 = _energyCharges.NBD0812;
                _energyCharges.NBD1620R = _energyCharges.NBD0812R;
                _energyCharges.NBD2024 = _energyCharges.NBD0812;
                _energyCharges.NBD2024R = _energyCharges.NBD0812R;
            }


            var _networkCharges = _repository.InvoiceSummaries.Where(a => a.InvoiceId == invoiceId).Select(b => b.NetworkCharge).FirstOrDefault();
            // var _energyCharges = _energyCharges.FirstOrDefault();//.Select(a => a.BD0004 + a.BD0408).FirstOrDefault();
            List<EnergyDataModel> _energyDataModel = new List<EnergyDataModel>();
            EnergyDataModel _businessDayData = new EnergyDataModel()
            {
                EnergyChargeByBracket = new List<decimal> { _energyCharges.BD0004, _energyCharges.BD0408, _energyCharges.BD0812, _energyCharges.BD1216, _energyCharges.BD1620, _energyCharges.BD2024 },
                EnergyRateByBracket = new List<decimal> { _energyCharges.BD0004R, _energyCharges.BD0408R, _energyCharges.BD0812R, _energyCharges.BD1216R, _energyCharges.BD1620R, _energyCharges.BD2024R },
                HeaderData = new HeaderData() { Header = "Business Days" }
            };

            EnergyDataModel _nonBusinessDayData = new EnergyDataModel()
            {
                EnergyChargeByBracket = new List<decimal> { _energyCharges.NBD0004, _energyCharges.NBD0408, _energyCharges.NBD0812, _energyCharges.NBD1216, _energyCharges.NBD1620, _energyCharges.NBD2024 },
                EnergyRateByBracket = new List<decimal> { _energyCharges.NBD0004R, _energyCharges.NBD0408R, _energyCharges.NBD0812R, _energyCharges.NBD1216R, _energyCharges.NBD1620R, _energyCharges.NBD2024R },
                HeaderData = new HeaderData() { Header = "Non Business Days" }
            };

            _result.OtherCharges = new List<decimal>() { _otherCharges.BDSVC, _otherCharges.NBDSVC, _otherCharges.EALevy, _invoiceDetail.MiscChargesTotal };
            _result.NetworkCharges = new List<decimal>() { _networkCharges.VariableBD, _networkCharges.VariableNBD, _networkCharges.CapacityCharge, _networkCharges.DemandCharge, _networkCharges.FixedCharge };

            var _zzMiscCharges = _result.InvoiceDetail.MiscChargesTotal;
            _result.InvoiceDetail.MiscChargesTotal = _result.InvoiceDetail.EnergyChargesTotal - _businessDayData.TotalCost - _nonBusinessDayData.TotalCost + _zzMiscCharges;
            _result.InvoiceDetail.EnergyChargesTotal = _businessDayData.TotalCost + _nonBusinessDayData.TotalCost;

            var _serviceCharges = new List<decimal> { _otherCharges.BDSVC, _otherCharges.BDSVCR };
            var _levyCharges = new List<decimal> { _otherCharges.EALevy, _otherCharges.EALevyR };
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
                                                    ApprovalData = new ApprovalData()
                                                    {
                                                        ApprovalDate = r.ApprovedDate,
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

        public InvoiceTallyViewModel GetInvoiceTally(string userId, int monthSpan)
        {
            if (!monthSpan.In(3, 6, 12, 24)) { monthSpan = 12; }

            DateTime _firstDate = GetFirstDateForSelect(monthSpan);

            InvoiceTallyViewModel _invoiceTally = new InvoiceTallyViewModel();
            SiteHierarchyViewModel _siteListForUser = GetSiteHierarchy(userId);
            var _siteIdList = _siteListForUser.SiteData.Select(s => s.SiteId);
            var _siteData = _repository.InvoiceSummaries.Where(s => _siteIdList.Contains(s.SiteId) & s.InvoiceDate > _firstDate);

            AutoMapper.Mapper.Map(_siteListForUser, _invoiceTally);
            _invoiceTally.InvoiceCosts = new List<InvoiceCosts>();

            CollateInvoiceDataBySiteId(_invoiceTally, _siteData, _firstDate);
            _invoiceTally.GroupCompanyName = _siteListForUser.HeaderName;
            _invoiceTally.MonthsOfData = monthSpan;

            return _invoiceTally;
        }

        private static DateTime GetFirstDateForSelect(int monthSpan)
        {
            DateTime _firstDate = DateTime.Now.AddMonths((monthSpan - 1) * -1);
            _firstDate = new DateTime(_firstDate.Year, _firstDate.Month, 1);
            return _firstDate;
        }



        #region update methods


        public InvoiceOverviewViewModel ApproveInvoice(int invoiceId, string userId)
        {
            InvoiceOverviewViewModel _result = new InvoiceOverviewViewModel();
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
            return _result;
        }


        #endregion
        #region private methods

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
                                      siteArea = (from f in g select f.Site.SiteArea).FirstOrDefault()
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
                    _match.CalculatedLossRate = Math.Round(_matchingResultForLosses.calculatedLosses,3);
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
            var now = DateTime.Now;
            int _monthsInPreviousYears = Math.Max(((now.Year - date.Year) * 12) - date.Month + 1, 0);
            int _monthsInThisYear = Math.Max(now.Month - date.Month,0);
            return (_monthsInPreviousYears + _monthsInThisYear);
        }

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

        private void checkInvoiceFileExists(List<InvoiceDetail> _invoicesDue)
        {
            // This process needs to be moved to invoice manmagement module, settting "exists" flag there
            System.Net.HttpWebResponse response = null;
            System.Net.HttpWebRequest request;
            string _sourcePdf;
            foreach (var _invoice in _invoicesDue)
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
