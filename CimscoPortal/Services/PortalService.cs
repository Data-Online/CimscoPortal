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

        public IEnumerable<MessageViewModel> GetNavbarDataForZ(int customerId, string pageElement)
        {
            var zz = _repo2.PortalMessages.Where(i => i.CustomerId == 3).ToList();
            return _repository.PortalMessages.Where(i => i.MessageFormat.MessageType.PageElement == pageElement && i.CustomerId == customerId)
                                            .Project().To<MessageViewModel>()
                                            .ToList();
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

            return _result;

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
            string _percentageChange = (zz / zzz).ToString("P", nfi);
            BarChartSummaryData _summaryData = new BarChartSummaryData() { Title = "ELECTRICITY COSTS", SubTitle = "Invoice History", PercentChange = _percentageChange };
            _result.MonthlyData = _data;
            _result.BarChartSummaryData = _summaryData;

            return _result;
        }

    }
}
