using CimscoPortal.Data;
using CimscoPortal.Infrastructure;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;


namespace CimscoPortal.Controllers
{
    public class PortalController : Controller
    {

        private IPortalService _portalService;

        //private readonly CimscoPortalEntities _context;

        //        public PortalController(CimscoPortalEntities context)
        public PortalController(IPortalService portalService)
        {
            this._portalService = portalService;
        }

        // GET: Portal
        public ActionResult Index()
        {
            // int categoryId = 2;
            //var zz = _portalService.GetAlertsFor(categoryId);
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

            return Json(DataSource, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPieChartData(string id)
        {

            //new List<AlertViewModel>()
            //     { 
            //        new AlertViewModel { CategoryName = "alert_tick_red", TypeName = "Alert" }
            //     });
            //var test = new List<Item>()
            //{
            //    new Item { data = 10.2M, label = "Energy", bars = new Bars { order=1, show=true, 
            //                                fillColor = new Fillcolor { colors = new Color[] { new Color { color = "red" }, new Color { color = "green" } } } } },

            //    new Item { data = 10.2M, label = "Energy", bars = new Bars { order=1, show=true, 
            //                                fillColor = new Fillcolor { colors = new Color[] { new Color { color = "yellow" }, new Color { color = "blue" } } } } }

            //};

            //fillColor = new Fillcolor  { colors = new Colors { color = "red" } },


            ////            {
            ////    label: "Energy",
            ////    data: d1_1,
            ////    bars: {
            ////        show: true,
            ////        order: 1,
            ////        fillColor: { colors: [{ color: chartthirdcolor }, { color: chartthirdcolor }] }
            ////    },
            ////    color: chartthirdcolor
            ////},
            MonthlySummaryViewModel model = new MonthlySummaryViewModel()
            {
                PieChartData = new List<PieChartData> { 
                    new PieChartData() { label="Energy", value=10968.34M },
                    new PieChartData() { label="Line", value=3540.45M },
                    new PieChartData() { label="Other", value=234.89M }
                },
                SummaryData = new SummaryDataZ
                {
                    TotalCharge = "$10,456.45",
                    Approved = 0,
                    DueDate = "26 Feb 2015",
                    Month = "FEBRUARY 2015"
                }
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DonutChartData(string id)
        {
            DonutChartViewModel model = new DonutChartViewModel()
            {
                DonutChartData = new List<DonutChartData>
                {
                    new DonutChartData() {label="Energy", value=10968.34M},
                    new DonutChartData() {label="Line", value=3540.45M},
                    new DonutChartData() {label="Other", value=234.89M}
                },
                SummaryData = new List<SummaryData>
                {
                    new SummaryData { Title="BILL TOTAL", Detail="$10,892.01"},
                    new SummaryData { Title="DUE DATE", Detail="2 Feb 2015"},
                },
                Header = "OCTOBER 2015",
                DataFor = id
            };

            MonthlySummaryViewModel modelz = new MonthlySummaryViewModel()
            {
                PieChartData = new List<PieChartData> { 
                    new PieChartData() { label="Energy", value=10968.34M },
                    new PieChartData() { label="Line", value=3540.45M },
                    new PieChartData() { label="Other", value=234.89M }
                },
                SummaryData = new SummaryDataZ
                {
                    TotalCharge = "$10,456.45",
                    Approved = 0,
                    DueDate = "26 Feb 2015",
                    Month = "FEBRUARY 2015"
                }
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMonthlyEnergySummary(string id)
        {
            StackedBarChartViewModel model = new StackedBarChartViewModel()
            {
                MonthlyData = new List<EnergyData> { 
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 156.89M, Month = "Jan" },
                    new EnergyData() { Energy = 20056.00M, Line = 5675.34M, Other = 1186.89M, Month = "Feb" }
                }
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}