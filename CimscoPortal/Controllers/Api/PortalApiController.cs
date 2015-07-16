using CimscoPortal.Infrastructure;
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

        public int ZZTest(int test)
        {
           var user = User.Identity.Name;
           var userId = User.Identity.GetUserId();

            if (user == "test")
                return 1;
            else
                return 2;

        }

        [HttpGet]
        [Route("messages/{customerId}")]
        public HttpResponseMessage GetMessages(HttpRequestMessage request, int customerId)
        {
            var messages = PortalDataFactory.GetMessages(customerId);
            return request.CreateResponse<MessageViewModel[]>(HttpStatusCode.OK, messages.ToArray());
        }

        [HttpGet]
        [Route("messagesZ/{customerId}")]
        public HttpResponseMessage GetMessagesZ(HttpRequestMessage request, int customerId)
        {
            //var service = PortalService(new IPortalService());
            //var messages = repo.MessageFormats.AsEnumerable().AsQueryable();
            var messages = _portalService.GetNavbarDataForZ(3, "pg-alert");
            return request.CreateResponse<MessageViewModel[]>(HttpStatusCode.OK, messages.ToArray());
        }

        [HttpGet]
        [Route("common")]
        public HttpResponseMessage GetCommonData(HttpRequestMessage request)
        {
            var userId = User.Identity.Name;
            return request.CreateResponse<CommonInfoViewModel>(HttpStatusCode.OK, _portalService.GetCommonData(userId));
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

        [HttpGet]
        [Route("sitehierarchy")]
        public HttpResponseMessage GetSiteHierarchy(HttpRequestMessage request)
        {
            var userId = User.Identity.Name;
            return request.CreateResponse<SiteHierarchyViewModel>(HttpStatusCode.OK, _portalService.GetSiteHierarchy(userId));
        }

        [HttpGet]
        [Route("siteinvoicedatafor/{siteId}")]
        public HttpResponseMessage GetCompanyInvoiceData(HttpRequestMessage request, int siteId)
        {
            var data = _portalService.GetSiteInvoiceData(siteId);
            return request.CreateResponse<InvoiceDetail[]>(HttpStatusCode.OK, data.ToArray());
        }

        [HttpGet]
        [Route("summarydata")]
        public HttpResponseMessage GetSummaryDataFor(HttpRequestMessage request)
        {
            var data = _portalService.GetSummaryDataFor(User.Identity.Name);
            var _return = request.CreateResponse<SummaryViewModel>(HttpStatusCode.OK, data);
            return _return;
        }

        #region Invoice data
        [HttpGet]
        [Route("invoicesummaryfor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceData(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.GetHistoryByMonth(invoiceId);
            return request.CreateResponse<StackedBarChartViewModel>(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("invoicedetailfor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceDetailData(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.GetCurrentMonth_(invoiceId);
            return request.CreateResponse<InvoiceDetailViewModel>(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("invoicedetailfor_/{invoiceId}")]
        public HttpResponseMessage GetCurrentMonth(HttpRequestMessage request, int invoiceId)   // Rename ?
        {
            var data = _portalService.GetCurrentMonth(invoiceId);
            return request.CreateResponse<InvoiceDetailViewModel_>(HttpStatusCode.OK, data);
        }

        [System.Web.Mvc.ValidateAntiForgeryToken]
        [HttpPost]
        [Route("invoiceapproval/{invoiceId}")]
        public HttpResponseMessage SetInvoiceApproved(HttpRequestMessage request, int invoiceId)
        {
            var userId = User.Identity.Name;
            _portalService.ApproveInvoice(invoiceId, userId);
            return request.CreateResponse(HttpStatusCode.OK);
        }

        #endregion


        public object CheckUserAccess(string UserName)
        {
            var user = User.Identity.Name;
            var userId = User.Identity.GetUserId();

            _portalService.ConfirmUserAccess(user);

            return true;
        }
    }
}
