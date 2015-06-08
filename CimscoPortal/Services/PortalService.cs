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
        ICimscoPortalEntities _repository;
        ICimscoPortalContext _repo2;

        public PortalService(ICimscoPortalEntities repository, ICimscoPortalContext repo2)
        {
            this._repository = repository;
            this._repo2 = repo2;
        }

        //public PortalService(ICimscoPortalContext repo2)
        //{
        //    this._repo2 = repo2;
        //}

        public DbSet<PortalMessage> PortalMessages
        {
            get { return _repository.PortalMessages; }
        }

        //public List<AlertViewModel> GetAlertsFor(int category)
        //{
        //    return _repository.PortalMessages.Where(i => i.MessageCategoryId == category)
        //                                    .Project().To<AlertViewModel>()
        //                                    .ToList();
        //}

        public CommonInfoViewModel GetCommonData()
        {
            var CommonData = new CommonInfoViewModel() { Temperature = "10", WeatherIcon = "wi wi-cloudy" };
            return CommonData;
        }

        public IEnumerable<MessageViewModel> GetNavbarDataForZ(int customerId, string pageElement)
        {
            return _repo2.PortalMessages.Where(i => i.MessageFormat.MessageType.PageElement == pageElement && i.CustomerId == customerId)
                                            .Project().To<MessageViewModel>()
                                            .ToList();
        }

        public CustomerHierarchyViewModel GetCompanyData(int contactId)
        {
            var _groupId = _repo2.Contacts.Where(i => i.ContactId == contactId).Select(s => s.Groups.Select(g => g.GroupId).ToList());
            return new CustomerHierarchyViewModel()
            {
                GroupName = "Test Group Name",
                CustomerData = new List<CustomerData> 
                                                    { new CustomerData { Address1 = "Addr1", CustomerName = "Customer Name", CustomerId = 1 },
                                                      new CustomerData { Address1 = "Addr2", CustomerName = "Customer Name 2", CustomerId = 2 } }
            };
            //return _repo2.Contacts.Where(i => i.ContactId == contactId)
            //                                .Project().To <CompanyDataViewModel>();
        }

        public IEnumerable<InvoiceDetail> GetCompanyInvoiceData(int contactId)
        {
            List<InvoiceDetail> _invoices = new List<InvoiceDetail>();
            if (contactId == 1)
            {
                _invoices.Add(new InvoiceDetail { Amount = 10352, DueDate = new DateTime(2014,1,1), PercentChange = 2, InvoiceId = 1 }); 
                _invoices.Add(new InvoiceDetail { Amount = 14362, DueDate = new DateTime(2014,2, 1) , PercentChange = 4, InvoiceId = 2 });
            }
            else
            {
                _invoices.Add(new InvoiceDetail { Amount = 13944, DueDate = new DateTime(2014, 1, 1), PercentChange = 3, InvoiceId = 3 });
                _invoices.Add(new InvoiceDetail { Amount = 19833, DueDate = new DateTime(2014, 2, 1), PercentChange = 4, InvoiceId = 4 });
            }
            return _invoices;
        }

        public SummaryViewModel GetSummaryDataFor(int customerId)
        {
            Random rnd = new Random();
            List<CompanyInvoiceViewModel> _invoiceData = new List<CompanyInvoiceViewModel>  {
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Jan", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Feb", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Mar", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Apr", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "May", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Jun", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "July", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Aug", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Sept", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Oct", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Nov", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=1, Month = "Dec",YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() }
            };
            List<CompanyInvoiceViewModel> _invoiceData2 = new List<CompanyInvoiceViewModel>  {
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Jan", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Feb", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Mar", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Apr", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "May", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Jun", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "July", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Aug", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Sept", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Oct", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Nov", YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() },
                                                        new CompanyInvoiceViewModel { CustomerId=2, Month = "Dec",YearA = rnd.Next(7000, 10000).ToString(), YearB = rnd.Next(7000, 10000).ToString() }
            };

            //var max = _invoiceData.Select(s => s.YearA).Max();

            CustomerHierarchyViewModel _customerData = new CustomerHierarchyViewModel()
            {
                GroupName = "Test Group Name",
                CustomerData = new List<CustomerData> 
                                                    { new CustomerData { CustomerId = 1, Address1 = "Addr1", CustomerName = "Customer Name" },
                                                      new CustomerData { CustomerId = 2, Address1 = "Addr2", CustomerName = "Customer Name 2" } }
            };
            SummaryViewModel model = new SummaryViewModel();
            model.CustomerHierarchy = _customerData;
            model.SummaryData = new List<InvoiceDataForCompany>();

            List<InvoiceDetail> _invoicesDue = new List<InvoiceDetail>() { 
                new InvoiceDetail { Amount = 10352, DueDate = new DateTime(2014,1,1), PercentChange = 2, InvoiceId = 1 }, 
                new InvoiceDetail { Amount = 14362, DueDate = new DateTime(2014,2, 1) , PercentChange = 4, InvoiceId = 2 } 
            };

            InvoiceDataForCompany zz = new InvoiceDataForCompany() { InvoiceHistory = _invoiceData, Year = 2014, InvoicesDue = _invoicesDue };
            model.SummaryData.Add(zz);
            _invoicesDue = new List<InvoiceDetail>() { 
                new InvoiceDetail { Amount = 11252, DueDate = new DateTime(2014,2,1), PercentChange = 1 }, 
                new InvoiceDetail { Amount = 13452, DueDate = new DateTime(2014,3, 1) , PercentChange = 0} 
            };

            zz = new InvoiceDataForCompany() { InvoiceHistory = _invoiceData2, Year = 2013, InvoicesDue = _invoicesDue };
            model.SummaryData.Add(zz);
            model.InvoicesDue = new List<InvoiceDetail>();
            model.InvoicesDue.Add(new InvoiceDetail() { Amount = 10352, DueDate = new DateTime(2014, 1, 1), PercentChange = 2, InvoiceId = 3 });
            model.InvoicesDue.Add(new InvoiceDetail() { Amount = 14362, DueDate = new DateTime(2014, 2, 1), PercentChange = 4, InvoiceId = 4 });

            model.MaxValue = 12000;

            return model;
        }

        public List<AlertData> GetNavbarDataFor(int customerId, string pageElement)
        {
            var zz = _repo2.PortalMessages.Where(i => i.CustomerId == 3).ToList();
            return _repository.PortalMessages.Where(i => i.MessageFormat.MessageType.PageElement == pageElement && i.CustomerId == customerId)
                                            .Project().To<AlertData>()
                                            .ToList();
        }

        public DonutChartViewModel GetCurrentMonth(int _energyPointId)
        {
            //string _dateFormat = "m";
            //GPA:  1. Region specific formats. 
            //      2. 
            IQueryable<DonutChartViewModel> q = from r in _repository.InvoiceSummaries.OrderByDescending(o => o.InvoiceDate)
                                                where (r.EnergyPointId == _energyPointId)
                                                select new DonutChartViewModel()
                                                {
                                                    DonutChartData = new List<DonutChartData> { new DonutChartData() { Value = r.TotalEnergyCharges, Label = "Energy" },
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
                                                                Detail = SqlFunctions.DateName("dd", r.InvoiceDate) + " " +  SqlFunctions.DateName("mm", r.InvoiceDate) + " "  + SqlFunctions.DateName("YY", r.InvoiceDate)}
                        }
                                                };

            var _result = q.FirstOrDefault();
            _result.SummaryData[0].Detail = Convert.ToDecimal(_result.SummaryData[0].Detail).ToString("C");

            ///
            var model = new List<EnergyDataModel>() {   
                new EnergyDataModel { 
                    EnergyChargesByBracket = new List<decimal> { 10.665M, 11.756M, 15.639M, 14.786M, 16.199M, 13.918M }, 
                    EnergyCostByBracket = new List<decimal> { 239.14M, 923.52M, 2344.94M, 2041.24M, 2136.30M, 300.30M }, 
                    HeaderData = new HeaderData { Header = "Weekday Costs" }
                },
                new EnergyDataModel {
                    EnergyChargesByBracket = new List<decimal> { 8.888M, 9.797M, 13.032M, 12.319M, 13.499M, 11.599M }, 
                    EnergyCostByBracket = new List<decimal> { 82.38M, 249.15M, 1036.01M, 927.74M, 572.29M, 115.38M }, 
                    HeaderData = new HeaderData { Header = "Weekend Costs" }
                }
            };
            ////

            return _result;

        }

        public InvoiceDetailViewModel GetCurrentMonth_(int _energyPointId)
        {
            //string _dateFormat = "m";
            //GPA:  1. Region specific formats. 
            //      2. 
            IQueryable<DonutChartViewModel> q = from r in _repository.InvoiceSummaries.OrderByDescending(o => o.InvoiceDate)
                                                where (r.EnergyPointId == _energyPointId)
                                                select new DonutChartViewModel()
                                                {
                                                    DonutChartData = new List<DonutChartData> { new DonutChartData() { Value = r.TotalEnergyCharges, Label = "Energy" },
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
                                                                Detail = SqlFunctions.DateName("dd", r.InvoiceDate) + " " +  SqlFunctions.DateName("mm", r.InvoiceDate) + " "  + SqlFunctions.DateName("YY", r.InvoiceDate)}
                        }
                                                };

            var _result = q.FirstOrDefault();
            _result.SummaryData[0].Detail = Convert.ToDecimal(_result.SummaryData[0].Detail).ToString("C");

            ///
            var model = new List<EnergyDataModel>() {   
                new EnergyDataModel { 
                    EnergyChargesByBracket = new List<decimal> { 10.665M, 11.756M, 15.639M, 14.786M, 16.199M, 13.918M }, 
                    EnergyCostByBracket = new List<decimal> { 239.14M, 923.52M, 2344.94M, 2041.24M, 2136.30M, 300.30M }, 
                    HeaderData = new HeaderData { Header = "Weekday Costs", DataFor = "1", _TempData = "WeeklyEnergyBySlice" }
                },
                new EnergyDataModel {
                    EnergyChargesByBracket = new List<decimal> { 8.888M, 9.797M, 13.032M, 12.319M, 13.499M, 11.599M }, 
                    EnergyCostByBracket = new List<decimal> { 82.38M, 249.15M, 1036.01M, 927.74M, 572.29M, 115.38M }, 
                    HeaderData = new HeaderData { Header = "Weekend Costs", DataFor = "2", _TempData = "WeekendEnergyBySlice" }
                }
            };
            ////
            InvoiceDetailViewModel returnData = new InvoiceDetailViewModel();

            returnData.ChartData = _result;
            returnData.EnergyCostData = model;

            return returnData;

        }

        public StackedBarChartViewModel GetHistoryByMonth(int _energyPointId)
        {
            var _result = new StackedBarChartViewModel();
            List<EnergyData> _data = _repository.InvoiceSummaries.Where(i => i.EnergyPointId == _energyPointId).OrderBy(o => o.InvoiceDate)
                                            .Project().To<EnergyData>()
                                            .ToList();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.PercentDecimalDigits = 0;
            decimal zz = _data.First().Energy;
            decimal zzz = _data.ElementAtOrDefault(_data.Count() - 13).Energy;  // .Last().Energy;
            decimal _percentageChange = (1 - (zz / zzz)); //.ToString("P", nfi);
            BarChartSummaryData _summaryData = new BarChartSummaryData() { Title = "ELECTRICITY COSTS", SubTitle = "Invoice History. Feb 2010 - Mar 2111", PercentChange = _percentageChange };

            _result.BarChartSummaryData = _summaryData;
            _result.MonthlyData = _data;

            for (int i = 0; i < _result.MonthlyData.Count(); i++)
            {
                _result.MonthlyData[i].Month = _result.MonthlyData[i]._month.ToString("MMM");
            }
            return _result;
        }

    }
}
