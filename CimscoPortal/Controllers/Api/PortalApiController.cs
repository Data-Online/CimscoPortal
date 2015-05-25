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

    }
}
