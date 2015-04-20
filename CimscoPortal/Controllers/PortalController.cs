//using CimscoPortal.Data;
using CimscoPortal.Infrastructure;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;

using Microsoft.AspNet.Identity;


namespace CimscoPortal.Controllers
{
    public class PortalController : CimscoPortalController
    {

        private IPortalService _portalService;
        //private string _userId;
       // private readonly ICurrentUser _currentUser;

        //private readonly CimscoPortalEntities _context;

        //        public PortalController(CimscoPortalEntities context)
        public PortalController(IPortalService portalService)//, ICurrentUser currentUser)
        {
            this._portalService = portalService;
            //this._userId = User.Identity.GetUserId();
          //  this._currentUser = currentUser;
        }
        
        // GET: Portal
        public ActionResult Index()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            
            return View();
            
        }
        public ActionResult Index_test()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);
            return View();
        }
        public JsonResult GetNavbarData(string id)
        {
            int customerId = 1;
            // var userName = User.Identity.Name;

            List<AlertViewModel> DataSource = _portalService.GetNavbarDataFor(customerId, id);

            return JsonSuccess(DataSource);
        }

        public JsonResult DonutChartData(string id)
        {
            //DonutChartViewModel model = new DonutChartViewModel()
            //{
            //    DonutChartData = new List<DonutChartData>
            //    {
            //        new DonutChartData() {Label="Energy", Value=10968.34M},
            //        new DonutChartData() {Label="Line", Value=3540.45M},
            //        new DonutChartData() {Label="Other", Value=234.89M}
            //    },
            //    SummaryData = new List<SummaryData>
            //    {
            //        new SummaryData { Title="BILL TOTAL", Detail="$10,892.01"},
            //        new SummaryData { Title="DUE DATE", Detail="2 Feb 2015"},
            //    },
            //    HeaderData = new HeaderData
            //    {
            //        Header = "SEPTEMBER 2015",
            //        DataFor = id
            //    },
            //};
            var userId = User.Identity.GetUserId();
            DonutChartViewModel model = _portalService.GetCurrentMonth(2);
            model.HeaderData.DataFor = "MonthlySummary";
            //return JsonSuccess(model);
            return JsonSuccess(model);
        }

        public JsonResult GetMonthlyEnergySummary(string id)
        {
            StackedBarChartViewModel model = new StackedBarChartViewModel()
            {
                MonthlyData = new List<EnergyData> { 
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "Jan" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Feb" },
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "March" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "April" },
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "May" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "June" },
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "July" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Aug" },
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "Sept" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Oct" },
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "Nov" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Dec" }

                },
                BarChartSummaryData = new BarChartSummaryData()
                {
                    PercentChange = "50%",
                    Title = "ELECTRICITY COSTS",
                    SubTitle = "Previous 6 Months"
                }
            };
            var ss = _portalService.GetHistoryByMonth(2);
            model.MonthlyData = ss;
            return JsonSuccess(model);
        }
    }
}