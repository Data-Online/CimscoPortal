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
            var test = new List<Item>()
            {
                new Item { data = 10.2M, label = "Energy", bars = new Bars { order=1, show=true, 
                                            fillColor = new Fillcolor { colors = new Color[] { new Color { color = "red" }, new Color { color = "green" } } } } },

                new Item { data = 10.2M, label = "Energy", bars = new Bars { order=1, show=true, 
                                            fillColor = new Fillcolor { colors = new Color[] { new Color { color = "yellow" }, new Color { color = "blue" } } } } }

            };

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


            return Json(test, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthlyEnergySummary(string id)
        {
            StackedBarChartViewModel model = new StackedBarChartViewModel()
            {
                MonthlyData = new IEnumerable<EnergyData>() 
                { 
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 56.89M, Month = "Jan" },
                    new EnergyData() { Energy = 10056.00M, Line = 4675.34M, Other = 56.89M, Month = "Jan" }
                }
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
      }
    }