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
                            _portalService.GetNavbarData(User.Identity.Name).ToArray());
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
                            _portalService.GetUserSettings(User.Identity.Name));
        }


        [System.Web.Mvc.ValidateAntiForgeryToken]
        [HttpPost]
        [Route("saveUserData")]
        public HttpResponseMessage SaveUserSettings(HttpRequestMessage request, [FromBody] UserSettingsViewModel userSettings)
        {
            //UserSettingsViewModel _data = _portalService.SaveUserData(userSettings, User.Identity.Name);
            if (_portalService.SaveUserSettings(userSettings, User.Identity.Name))
                return request.CreateResponse(HttpStatusCode.OK);
            else
                return request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("filters")]
        public HttpResponseMessage GetAllFilters(HttpRequestMessage request)
        {
            return request.CreateResponse<AvailableFiltersModel>(HttpStatusCode.OK,
                            _portalService.GetAllFilters(User.Identity.Name));
        }

        [HttpGet]
        [Route("welcomeScreen")]
        public HttpResponseMessage GetWelcomeScreen(HttpRequestMessage request)
        {
            return request.CreateResponse<TextViewModel>(HttpStatusCode.OK,
                            _portalService.GetWelcomeScreen(User.Identity.Name));
        }

        //[HttpGet]
        //[Route("TotalCostsByMonth/{monthSpan}/{customerId}")]
        //public HttpResponseMessage GetTotalCostsByMonth(HttpRequestMessage request, int monthSpan, int customerId)
        //{
        //    var data = _portalService.GetTotalCostsByMonth(User.Identity.Name, monthSpan, customerId);
        //    return request.CreateResponse<DashboardViewData>(HttpStatusCode.OK, data);
        //}

        //[HttpGet]
        //[Route("TotalCostsByMonth/{monthSpan}")]
        //public HttpResponseMessage GetTotalCostsByMonth(HttpRequestMessage request, int monthSpan)
        //{
        //    var data = _portalService.GetTotalCostsByMonth(User.Identity.Name, monthSpan, null);
        //    return request.CreateResponse<DashboardViewData>(HttpStatusCode.OK, data);
        //}

        [HttpGet]
        [Route("siteDetails/{siteId}")]
        public HttpResponseMessage GetSiteDetails(HttpRequestMessage request, int siteId)
        {
            if (GetCurrentUserAccess().ValidSites.Contains(siteId))
            {
                var _data = _portalService.GetSiteDetails(siteId);
                return request.CreateResponse<SiteDetailViewModel>(HttpStatusCode.OK,
                                _portalService.GetSiteDetails(siteId));
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }

        [HttpGet]
        [Route("costAndConsumption/{monthSpan}/{filter}/{siteId}")]
        public HttpResponseMessage GetCostsAndConsumption(HttpRequestMessage request, int monthSpan, string filter, int siteId)
        {
            if (CheckUserAccessToSite(siteId))
            //if (GetCurrentUserAccess().ValidSites.Contains(siteId))
            {
                CostConsumptionOptions _options = new CostConsumptionOptions();
                _options.userId = User.Identity.Name;
                _options.siteId = siteId;
                _options.filter = filter;
                _options.includeMissing = true;
                _options.previous12 = true;

                //var data = _portalService.GetCostsAndConsumption(User.Identity.Name, monthSpan, siteId);
                //GoogleChartViewModel data = _portalService.GetCostsAndConsumption(monthSpan, _options);
                return request.CreateResponse<GoogleChartViewModel>(HttpStatusCode.OK,
                    _portalService.GetCostsAndConsumption(monthSpan, _options));
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }

        [HttpGet]
        [Route("comparisonData/{monthSpan}/{filter}/{siteId}")]
        public HttpResponseMessage GetComparisonData(HttpRequestMessage request, int monthSpan, string filter, int siteId)
        {
            if (CheckUserAccessToSite(siteId))
            //if (GetCurrentUserAccess().ValidSites.Contains(siteId))
            {
                CostConsumptionOptions _options = new CostConsumptionOptions();
                _options.userId = User.Identity.Name;
                _options.siteId = siteId;
                _options.filter = filter;
                _options.includeMissing = true;
                _options.previous12 = true;
                return request.CreateResponse<GoogleChartViewModel>(HttpStatusCode.OK,
                   _portalService.GetComparisonData(monthSpan, _options));
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }


        //[HttpGet]
        //[Route("CostAndConsumption/{siteId}/{monthSpan}/{filter}")]
        //public HttpResponseMessage GetCostsAndConsumption(HttpRequestMessage request, int siteId, int monthSpan)
        //{
        //    if (GetCurrentUserAccess().ValidSites.Contains(siteId))
        //    {
        //        CostConsumptionOptions _options = new CostConsumptionOptions();
        //        _options.siteId = siteId;
        //        _options.includeMissing = true;
        //        _options.previous12 = true;

        //        var data = _portalService.GetCostsAndConsumption(User.Identity.Name, monthSpan, siteId);
        //        var data_ = _portalService.GetCostsAndConsumption_(monthSpan, _options);
        //        return request.CreateResponse<GoogleChartViewModel>(HttpStatusCode.OK, data_);
        //    }
        //    return request.CreateResponse(HttpStatusCode.Forbidden);
        //}

        //[HttpGet]
        //[Route("CostAndConsumption/{monthSpan}/{filter}")]
        //public HttpResponseMessage GetCostsAndConsumption(HttpRequestMessage request, int monthSpan, string filter)
        //{
        //    CostConsumptionOptions _options = new CostConsumptionOptions();
        //    _options.userId = User.Identity.Name;
        //    _options.filter = filter;
        //    _options.includeMissing = true;
        //    _options.previous12 = true;

        //    var data_ = _portalService.GetCostsAndConsumption_(monthSpan, _options);
        //    var data = _portalService.GetCostsAndConsumption(User.Identity.Name, monthSpan, filter);
        //    return request.CreateResponse<GoogleChartViewModel>(HttpStatusCode.OK, data_);
        //}

        //[HttpGet]
        //[Route("TotalCostAndConsumption/{monthSpan}/{filter}")]
        //public HttpResponseMessage GetTotalCostsAndConsumption(HttpRequestMessage request, int monthSpan, string filter)
        //{
        //    var data = _portalService.GetTotalCostsAndConsumption(User.Identity.Name, monthSpan, filter);
        //    return request.CreateResponse<DashboardViewData>(HttpStatusCode.OK, data);
        //}

        //[HttpGet]
        //[Route("TotalCostAndConsumption/{monthSpan}/{filter}/{test}")]
        //public HttpResponseMessage GetTotalCostsAndConsumption_(HttpRequestMessage request, int monthSpan, string filter, string test)
        //{
        //    var data = _portalService.GetTotalCostsAndConsumption(User.Identity.Name, monthSpan, filter);
        //    return request.CreateResponse<DashboardViewData>(HttpStatusCode.OK, data);
        //}

        [HttpGet]
        [Route("dashboardStatistics/{monthSpan}/{filter}")]
        public HttpResponseMessage GetDashboardStatistics(HttpRequestMessage request, int monthSpan, string filter)
        {
            var data = _portalService.GetDashboardStatistics(User.Identity.Name, monthSpan, filter);
            return request.CreateResponse<InvoiceStatsViewModel>(HttpStatusCode.OK, data);
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
        //[HttpGet]
        //[Route("sitehierarchy")]
        //public HttpResponseMessage GetSiteHierarchy(HttpRequestMessage request)
        //{
        //    var userId = User.Identity.Name;
        //    return request.CreateResponse<SiteHierarchyViewModel>(HttpStatusCode.OK, _portalService.GetSiteHierarchy(userId));
        //}
        #region Invoices

        [HttpGet]
        [Route("siteInvoiceDataFor/{siteId}")]
        public HttpResponseMessage GetSiteInvoiceData(HttpRequestMessage request, int siteId)
        {
            if (CheckUserAccessToSite(siteId))
            {
                var data = _portalService.GetInvoiceDetailForSite(siteId);
                return request.CreateResponse<InvoiceDetail[]>(HttpStatusCode.OK, data.ToArray());
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }

        [HttpGet]
        [Route("siteHistoryDataFor/{invoiceId}")]
        public HttpResponseMessage GetSiteHistoricalData(HttpRequestMessage request, int invoiceId)
        {
            // Need check to ensure user can view this data invId --> siteId --> check
            var data = _portalService.GetHistoricalDataForSite(invoiceId);
            return request.CreateResponse<MonthlyConsumptionViewModal[]>(HttpStatusCode.OK, data.ToArray());
        }


        [HttpGet]
        [Route("invoiceOverviewFor/{siteId}")]
        public HttpResponseMessage GetInvoiceOverview(HttpRequestMessage request, int siteId)
        {
            if (CheckUserAccessToSite(siteId))
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
            if (CheckUserAccessToSite(siteId))
            {
                var data = _portalService.GetInvoiceOverviewForSite(siteId, monthsToDisplay);
                return request.CreateResponse<InvoiceOverviewViewModel[]>(HttpStatusCode.OK, data.ToArray());
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }


        [HttpGet]
        [Route("invoiceAllOverview/{monthSpan}/{filter}/{pageNo}")]
        public HttpResponseMessage GetInvoiceOverview(HttpRequestMessage request, int monthSpan, string filter, int pageNo)
        {
            //return request.CreateResponse(HttpStatusCode.Forbidden);
            var data = _portalService.GetAllInvoiceOverview(User.Identity.Name, monthSpan, filter, pageNo);
            return request.CreateResponse<InvoiceOverviewViewModel[]>(HttpStatusCode.OK, data.ToArray());
        }

        [HttpGet]
        [Route("invoiceSummaryFor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceSummary(HttpRequestMessage request, int invoiceId)
        {
            var data = _portalService.InvoiceSummaryByMonth(invoiceId);
            return request.CreateResponse<StackedBarChartViewModel>(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("invoiceDetailFor/{invoiceId}")]
        public HttpResponseMessage GetInvoiceDetail(HttpRequestMessage request, int invoiceId)   // Rename ?
        {
            var data = _portalService.GetInvoiceDetail(invoiceId);
            return request.CreateResponse<InvoiceDetailViewModel>(HttpStatusCode.OK, data);
        }

        #endregion

        [HttpGet]
        [Route("summarydata")]
        public HttpResponseMessage GetSummaryDataFor(HttpRequestMessage request)
        {
            var data = _portalService.GetSummaryDataFor(User.Identity.Name);
            return request.CreateResponse<SummaryViewModel>(HttpStatusCode.OK, data);
        }

        #region Invoice data
        //[HttpGet]
        //[Route("invoicetally/{monthSpan}")]
        //public HttpResponseMessage GetInvoiceTally(HttpRequestMessage request, int monthSpan)
        //{
        //    var data = _portalService.GetInvoiceTally(User.Identity.Name, monthSpan, null);
        //    return request.CreateResponse<InvoiceTallyViewModel>(HttpStatusCode.OK, data);
        //}

        [HttpGet]
        [Route("detailbysite/{monthSpan}/{filter}")]
        public HttpResponseMessage GetDetailBySite(HttpRequestMessage request, int monthSpan, string filter)
        {
            var data = _portalService.GetDetailBySite(User.Identity.Name, monthSpan, filter);
            return request.CreateResponse<DetailBySiteViewModel>(HttpStatusCode.OK, data);
        }

        //[HttpGet]
        //[Route("invoicetally/{monthSpan}/{customerId}")]
        //public HttpResponseMessage GetInvoiceTally(HttpRequestMessage request, int monthSpan, int customerId)
        //{
        //    var data = _portalService.GetInvoiceTally(User.Identity.Name, monthSpan, customerId);
        //    return request.CreateResponse<InvoiceTallyViewModel>(HttpStatusCode.OK, data);
        //}



        [HttpPost]
        [Route("DatapointDetails/{siteId}")]
        public HttpResponseMessage GetDatapointDetails(HttpRequestMessage request, [FromBody] DatapointIdentity datapointId, int siteId)
        {
            if (CheckUserAccessToSite(siteId))
            {
                CostConsumptionOptions _options = new CostConsumptionOptions();
                _options.userId = User.Identity.Name;
                _options.siteId = siteId;
                //_options.filter = filter;
                _options.includeMissing = true;
                //_options.previous12 = true;
                DatapointDetailView _response = _portalService.GetDatapointDetails(datapointId, _options);
                return request.CreateResponse(HttpStatusCode.OK, _response);
            }
            return request.CreateResponse(HttpStatusCode.Forbidden);
        }

        [System.Web.Mvc.ValidateAntiForgeryToken]
        [HttpPost]
        [Route("invoiceApproval/{invoiceId}")]
        public HttpResponseMessage SetInvoiceApproved(HttpRequestMessage request, int invoiceId)
        {
            if (CheckUserAccessToInvoice(User.Identity.Name, invoiceId))
            {
                string _rootUrl = GetRootUrl();
                var _data = _portalService.ApproveInvoice(invoiceId, User.Identity.Name, _rootUrl);
                if (_data.Approved)
                    return request.CreateResponse<InvoiceOverviewViewModel>(HttpStatusCode.OK, _data);
                else
                    return request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest);
        }



        [System.Web.Mvc.ValidateAntiForgeryToken]
        [HttpGet]
        [Route("invoiceStatsBySite/{monthSpan}/{filter}")]
        public HttpResponseMessage GetInvoiceStatsForSites(HttpRequestMessage request, int monthSpan, string filter)
        {
            var data = _portalService.GetInvoiceStatsForSites(User.Identity.Name, monthSpan, filter);
            return request.CreateResponse<InvoiceStatsBySiteViewModel[]>(HttpStatusCode.OK, data.ToArray());
        }

        [System.Web.Mvc.ValidateAntiForgeryToken]
        [HttpPost]
        [Route("feedback")]
        public System.Web.Mvc.JsonResult Feedback(HttpRequestMessage request, object data)
        //public HttpResponseMessage Feedback(HttpRequestMessage request, object data)
        {
            var result = new System.Web.Mvc.JsonResult();
            result.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;

            if (_portalService.LogFeedback(data, User.Identity.Name))
            {
                result.Data = JsonResponseFactory.SuccessResponse();
            }
            else
            {
                result.Data = JsonResponseFactory.ErrorResponse("Unable to save feedback");
            }
            return result;
        }

        #endregion

        public class JsonResponseFactory
        {
            public static object ErrorResponse(string error)
            {
                return new { success = false, ErrorMessage = error };
            }

            public static object SuccessResponse()
            {
                return new { success = true };
            }

            public static object SuccessResponse(object referenceObject)
            {
                return new { Success = true, Object = referenceObject };
            }

        }

        public UserAccessModel GetCurrentUserAccess()
        {
            var _user = User.Identity.Name;

            UserAccessModel UserAccessModel = new UserAccessModel();

            UserAccessModel = _portalService.CheckUserAccess(_user);

            return UserAccessModel;
        }

        private bool CheckUserAccessToSite(int siteId)
        {
            if (siteId == 0)
            {
                return true;
            }
            return GetCurrentUserAccess().ValidSites.Contains(siteId);
        }

        public bool CheckUserAccessToInvoice(string userId, int invoiceId)
        {
            return _portalService.CheckUserAccessToInvoice(userId, invoiceId);
        }

        public static string GetRootUrl()
        {
            return "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port;
        }
    }
}
