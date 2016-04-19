﻿using CimscoPortal.Infrastructure;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity;

namespace CimscoPortal.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api")]
    public class PortalApiController : ApiController
    {
        private IPortalService _portalService;
        public PortalApiController(IPortalService portalService)
        {
            this._portalService = portalService;
        }

        public int ZZTest()
        {
            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();

            if (user == "test")
                return 1;
            else
                return 2;

        }

        [HttpGet]
        [Route("messages")]
        public HttpResponseMessage GetMessages(HttpRequestMessage request)
        {
            return request.CreateResponse<MessageViewModel[]>(HttpStatusCode.OK,
                            _portalService.GetNavbarDataFor(User.Identity.Name).ToArray());
        }

        [HttpGet]
        [Route("common")]
        public HttpResponseMessage GetCommonData(HttpRequestMessage request)
        {
            return request.CreateResponse<CommonInfoViewModel>(HttpStatusCode.OK,
                            _portalService.GetCommonData(User.Identity.Name));
        }

        [HttpGet]
        [Route("userdata")]
        public HttpResponseMessage GetUserSettings(HttpRequestMessage request)
        {
            return request.CreateResponse<UserSettingsViewModel>(HttpStatusCode.OK,
                            _portalService.GetUserSettingsFor(User.Identity.Name));
        }

        //[HttpGet]
        //[Route("companyhierarchy")]
        //public HttpResponseMessage GetCompanyData(HttpRequestMessage request)
        //{
        //    var userId = User.Identity.Name;
        //    //var zz = User.IsInRole("Admin");
        //    //var userId = User.Identity.GetUserId();
        //    return request.CreateResponse<CustomerHierarchyViewModel>(HttpStatusCode.OK, _portalService.GetCompanyHierarchy(userId));
        //}

        // --> GPA ** This is only used in tests - logic now in Service layer. Move tests to there.
        [HttpGet]
        [Route("sitehierarchy")]
        public HttpResponseMessage GetSiteHierarchy(HttpRequestMessage request)
        {
            var userId = User.Identity.Name;
            return request.CreateResponse<SiteHierarchyViewModel>(HttpStatusCode.OK, _portalService.GetSiteHierarchy(userId));
        }

        [HttpGet]
        [Route("siteinvoicedatafor/{siteId}")]
        public HttpResponseMessage GetSiteInvoiceData(HttpRequestMessage request, int siteId)
        {
            var data = _portalService.GetInvoiceDetailForSite(siteId);
            return request.CreateResponse<InvoiceDetail[]>(HttpStatusCode.OK, data.ToArray());
        }

        [HttpGet]
        [Route("sitehistorydatafor/{invoiceId}")]
        public HttpResponseMessage GetSiteHistoricalData(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.GetHistoricalDataForSite(invoiceId);
            return request.CreateResponse<MonthlyConsumptionViewModal[]>(HttpStatusCode.OK, data.ToArray());
        }

        
        [HttpGet]
        [Route("invoiceOverviewFor/{siteId}")]
        public HttpResponseMessage GetInvoiceOverview(HttpRequestMessage request, int siteId)
        {
            if (GetCurrentUserAccess().ValidSites.Contains(siteId))
            { 
                var data = _portalService.GetInvoiceOverviewForSite(siteId);
                return request.CreateResponse<InvoiceOverviewViewModel[]>(HttpStatusCode.OK, data.ToArray());
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);            
        }

        [HttpGet]
        [Route("invoiceOverviewFor/{siteId}/{monthsToDisplay}")]
        public HttpResponseMessage GetInvoiceOverview(HttpRequestMessage request, int siteId, int monthsToDisplay)
        {
            if (GetCurrentUserAccess().ValidSites.Contains(siteId))
            {
                var data = _portalService.GetInvoiceOverviewForSite(siteId, monthsToDisplay);
                return request.CreateResponse<InvoiceOverviewViewModel[]>(HttpStatusCode.OK, data.ToArray());
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }

        [HttpGet]
        [Route("summarydata")]
        public HttpResponseMessage GetSummaryDataFor(HttpRequestMessage request)
        {
            var data = _portalService.GetSummaryDataFor(User.Identity.Name);
            return request.CreateResponse<SummaryViewModel>(HttpStatusCode.OK, data);
        }

        #region Invoice data
        [HttpGet]
        [Route("invoicetally/{monthSpan}")]
        public HttpResponseMessage GetInvoiceTally(HttpRequestMessage request, int monthSpan)
        {
            var data = _portalService.GetInvoiceTally(User.Identity.Name, monthSpan, null);
            return request.CreateResponse<InvoiceTallyViewModel>(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("invoicetally/{monthSpan}/{customerId}")]
        public HttpResponseMessage GetInvoiceTally(HttpRequestMessage request, int monthSpan, int customerId)
        {
            var data = _portalService.GetInvoiceTally(User.Identity.Name, monthSpan, customerId);
            return request.CreateResponse<InvoiceTallyViewModel>(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("invoicesummaryfor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceData(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.GetHistoryByMonth(invoiceId);
            return request.CreateResponse<StackedBarChartViewModel>(HttpStatusCode.OK, data);
        }

        //[HttpGet]
        //[Route("invoicedetailfor/{invoiceId}")]
        //public HttpResponseMessage GetInvoiceDetailData(HttpRequestMessage request, int invoiceId)
        //{
        //    var data = _portalService.GetCurrentMonth_(invoiceId);
        //    return request.CreateResponse<InvoiceDetailViewModel>(HttpStatusCode.OK, data);
        //}

        [HttpGet]
        [Route("invoicedetailfor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceDetail(HttpRequestMessage request, int invoiceId)   // Rename ?
        {
            var data = _portalService.GetInvoiceDetail(invoiceId);
            return request.CreateResponse<InvoiceDetailViewModel>(HttpStatusCode.OK, data);
        }

        [System.Web.Mvc.ValidateAntiForgeryToken]
        [HttpPost]
        [Route("invoiceApproval/{invoiceId}")]
        public HttpResponseMessage SetInvoiceApproved(HttpRequestMessage request, int invoiceId)
        {
            var _rootUrl = System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port;
            var _data = _portalService.ApproveInvoice(invoiceId, User.Identity.Name, _rootUrl);
            if (_data.Approved)
                return request.CreateResponse<InvoiceOverviewViewModel>(HttpStatusCode.OK, _data);
            else
                return request.CreateResponse(HttpStatusCode.BadRequest);
        }

        #endregion


        public UserAccessModel GetCurrentUserAccess()
        {
            var _user = User.Identity.Name;

            UserAccessModel UserAccessModel = new UserAccessModel();

            UserAccessModel = _portalService.CheckUserAccess(_user);

            return UserAccessModel;
        }
    }
}
