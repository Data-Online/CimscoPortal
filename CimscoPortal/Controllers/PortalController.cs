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
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;


namespace CimscoPortal.Controllers
{
    [Authorize]
    public class PortalController : CimscoPortalController
    {

        Dictionary<string, CloudBlockBlob> dicBBlob = new Dictionary<string, CloudBlockBlob>();
        Dictionary<string, CloudPageBlob> dicPBlob = new Dictionary<string, CloudPageBlob>();
        Dictionary<string, List<string>> dicSelectedBlob = new Dictionary<string, List<string>>();
        List<string> lstContainer = new List<string>();

        CloudStorageAccount csa_storageAccount;


        public CloudStorageAccount Csa_storageAccount
        {
            get
            {
                if (csa_storageAccount == null)
                {
                    string strAccount = "cimsco";
                    string strKey = "rblKAeB7iHoOf4zCbZL15TBLTpSP3kOwDCN2L8nHzrkY/+rRH/lwpW3Qu1FSpLwIjKFPbBT+SgOGffIQZKYH1w==";

                    StorageCredentials credential = new StorageCredentials(strAccount, strKey);
                    csa_storageAccount = new CloudStorageAccount(credential, true);
                }
                return csa_storageAccount;
            }
        }


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
        
        // TEST
        public ActionResult Index2_()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            var zz = User.IsInRole("Admin");
            return View();
        }
        public ActionResult Index2()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            var zz = User.IsInRole("Admin");
            return View();
        }

        // GET: Portal
        public ActionResult Index()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            var zz = User.IsInRole("Admin");
            return View("Dashboard");            
        }

        public ActionResult Dashboard()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            var zz = User.IsInRole("Admin");
            return View();
        }

        public ActionResult SitesOverview()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            var zz = User.IsInRole("Admin");
            return View();
        }

        public ActionResult DetailBySite()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            //var user = User.Identity.Name;
            //var userId = User.Identity.GetUserId();
            //var zz = User.IsInRole("Admin");
            return View();
        }


        public ActionResult SiteOverview(int? id)
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            var zz = User.IsInRole("Admin");
            ViewBag.siteId = id ?? 0;
            return View();
        }

        public ActionResult SiteSummary(string id)
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);

            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();

            return View();
        }

        public ActionResult InvoiceOverview()
        {
            return View();
        }

        public ActionResult InvoiceDetail(string id)
        {
            ViewBag.InvoiceId = id;
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
            string pageElement = id;
            // var userName = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            AlertViewModel model = new AlertViewModel();
            List<AlertData> alertData = _portalService.GetNavbarDataFor_Z(customerId, pageElement);

            if (alertData.Count() > 0)
            {
                alertData[0].TimeStamp = alertData[0]._timeStamp.ToString("m");
            }
            model.AlertData = alertData;
            model.HeaderData = new HeaderData() { DataFor = pageElement };

            return JsonSuccess(model);
        }

        ////public JsonResult DonutChartData(string id)
        ////{
        ////    //DonutChartViewModel model = new DonutChartViewModel()
        ////    //{
        ////    //    DonutChartData = new List<DonutChartData>
        ////    //    {
        ////    //        new DonutChartData() {Label="Energy", Value=10968.34M},
        ////    //        new DonutChartData() {Label="Line", Value=3540.45M},
        ////    //        new DonutChartData() {Label="Other", Value=234.89M}
        ////    //    },
        ////    //    SummaryData = new List<SummaryData>
        ////    //    {
        ////    //        new SummaryData { Title="BILL TOTAL", Detail="$10,892.01"},
        ////    //        new SummaryData { Title="DUE DATE", Detail="2 Feb 2015"},
        ////    //    },
        ////    //    HeaderData = new HeaderData
        ////    //    {
        ////    //        Header = "SEPTEMBER 2015",
        ////    //        DataFor = id
        ////    //    },
        ////    //};
        ////    var userId = User.Identity.GetUserId();
        ////    DonutChartViewModel model = new DonutChartViewModel();//_portalService.GetCurrentMonth(2);
        ////    model.HeaderData.DataFor = "MonthlySummary";
        ////    return JsonSuccess(model);
        ////}

        public JsonResult GetMonthlyEnergySummary(string id)
        {
            StackedBarChartViewModel model = new StackedBarChartViewModel()
            {
                //MonthlyData = new List<EnergyData> { 
                //    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M,  Month = DateTime.Parse("02-01-2011").ToString("MMM"),  },
                //    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Feb" },
                //    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "March" },
                //    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "April" },
                //    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "May" },
                //    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "June" },
                //    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "July" },
                //    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Aug" },
                //    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "Sept" },
                //    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Oct" },
                //    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "Nov" },
                //    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Dec" }

                //},
                BarChartSummaryData = new BarChartSummaryData()
                {
                    //PercentChange = "50%",
                    PercentChange = 50.00M,
                    Title = "ELECTRICITY COSTS",
                    SubTitle = "Bill History"
                }
            };
            var ss = _portalService.GetHistoryByMonth(2);
           // model.MonthlyData = ss;
            //for (int i = 0; i < ss.MonthlyData.Count(); i++)
            //{
            //    ss.MonthlyData[i].Month = ss.MonthlyData[i]._month.ToString("MMM");
            //}
            return JsonSuccess(ss);
        }

        [HttpGet]
        public JsonResult GetSparklineDataFor(string id)
        {
            id = "WeeklyEnergyBySlice";
            var model = new EnergyDataModel() {};
            model.HeaderData = new HeaderData();
            switch (id)
            {
                case "WeeklyEnergyBySlice":
                    model.EnergyChargeByBracket = new List<decimal> { 10.665M, 11.756M, 15.639M, 14.786M, 16.199M, 13.918M };
                    model.EnergyRateByBracket = new List<decimal> { 239.14M, 923.52M, 2344.94M, 2041.24M, 2136.30M, 300.30M };
                    model.HeaderData.Header = "Weekday Costs";
                    break;
                case "WeekendEnergyBySlice":
                    model.EnergyChargeByBracket = new List<decimal> { 8.888M, 9.797M, 13.032M, 12.319M, 13.499M, 11.599M };
                    model.EnergyRateByBracket = new List<decimal> { 82.38M, 249.15M, 1036.01M, 927.74M, 572.29M, 115.38M };
                    model.HeaderData.Header = "Weekend Costs";
                    break;
            };
            model.HeaderData.DataFor = id;
            var zz = model.MaxCharge;
            return JsonSuccess(model);
        }

        [HttpGet]
        public JsonResult GetData(string id)
        {

            // id == <model_base_name>_<dimension> || MonthTotals_1 == MonthTotalsViewModel dimension 1
            var sparklist = new System.Collections.Generic.List<Object>();
            int max = 12; int min = 1;

            switch (id)
            {
                case "MonthTotals_1":
                    max = 20; min = 10;
                    break;
                case "MonthTotals_2":
                    max = 30; min = 20;
                    break;
                default:
                    max = 8; min = 1;
                    break;
            }

            for (int loop = min; loop <= max; loop++)
            {
                sparklist.Add(loop);
            }
            return JsonSuccess(sparklist);
        }
    }
}