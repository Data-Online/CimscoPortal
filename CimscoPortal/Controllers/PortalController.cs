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

    }
}