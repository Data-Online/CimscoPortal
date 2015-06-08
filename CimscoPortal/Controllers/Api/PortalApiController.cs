using CimscoPortal.Infrastructure;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            return request.CreateResponse<CommonInfoViewModel>(HttpStatusCode.OK, _portalService.GetCommonData());
        }

        [HttpGet]
        [Route("companylistfor/{customerId}")]
        public HttpResponseMessage GetCompanyData(HttpRequestMessage request, int customerId)
        {
            return request.CreateResponse<CustomerHierarchyViewModel>(HttpStatusCode.OK, _portalService.GetCompanyData(customerId));
        }

        [HttpGet]
        [Route("companyinvoicedatafor/{customerId}")]
        public HttpResponseMessage GetCompanyInvoiceData(HttpRequestMessage request, int customerId)
        {
            var data = _portalService.GetCompanyInvoiceData(customerId);
            return request.CreateResponse<InvoiceDetail[]>(HttpStatusCode.OK, data.ToArray());
        }

        [HttpGet]
        [Route("summarydatafor/{customerId}")]
        public HttpResponseMessage GetSummaryDataFor(HttpRequestMessage request, int customerId)
        {
            var data = _portalService.GetSummaryDataFor(customerId);
            return request.CreateResponse<SummaryViewModel>(HttpStatusCode.OK, data);
        }

        #region Invoice data
        [HttpGet]
        [Route("invoicesummaryfor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceData(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.GetHistoryByMonth(2);
            return request.CreateResponse<StackedBarChartViewModel>(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("invoicedetailfor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceDetailData(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.GetCurrentMonth_(2);
            return request.CreateResponse<InvoiceDetailViewModel>(HttpStatusCode.OK, data);
        }

        #endregion

    }
}
