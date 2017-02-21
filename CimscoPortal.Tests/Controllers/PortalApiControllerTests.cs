using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CimscoPortal.Infrastructure;
using Telerik.JustMock;
using CimscoPortal.Controllers.Api;
using System.Web.Mvc;
using CimscoPortal.Models;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

namespace CimscoPortal.Tests.Controllers
{
    [TestClass]
    public class PortalAPiControllerTests
    {

        #region Valid Tests

        #region common settings

        private static List<int> validSites = new List<int>(new int[] { 1, 2, 3 });
        private static List<int> validInvoices = new List<int>(new int[] { 1, 2, 3 });
        private static List<int> invalidInvoices = new List<int>(new int[] { 10, 20, 30 });
        private static string standardUser = "standardUser";
        private static string invalidUser = "invalidUser";
        //private static string approvedUser = "approvedUser";
        private static string apiAddress = "http://localhost/api/";

        private static UserAccessModel SetupUserAccess(string userId)
        {
            UserAccessModel _validationDetails = new UserAccessModel();

            switch (userId)
            {
                case "standardUser":
                    _validationDetails.ViewInvoices = true;
                    _validationDetails.ValidSites = validSites;
                    break;
                case "approvedUser":
                    _validationDetails.ViewInvoices = true;
                    _validationDetails.ValidSites = validSites;
                    break;
                case "invalidUser":
                    _validationDetails.ViewInvoices = false;
                    _validationDetails.ValidSites = new List<int>(new int[] { });
                    break;
                default: break;
            }

            return _validationDetails;
        }
        #endregion

        #region GetMessages
        [TestMethod]
        public void GetMessages_ReturnsModelForCurrentUser()
        {

            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "messages") };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);

            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.GetNavbarData(standardUser)).Returns(new List<MessageViewModel>());

            // Act
            HttpResponseMessage _response = _controller.GetMessages(_request);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(new List<MessageViewModel>()), _result);

        }
        #endregion

        #region GetCommonData
        [TestMethod]
        public void GetCommonData_ReturnsModelForCurrentUser()
        {
            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "common") };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);

            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.GetCommonData(standardUser)).Returns(new CommonInfoViewModel());

            // Act
            HttpResponseMessage _response = _controller.GetCommonData(_request);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(new CommonInfoViewModel()), _result);

        }
        #endregion

        #region GetUserSettings
        [TestMethod]
        public void GetUserSettings_ReturnsModelForCurrentUser()
        {
            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "userdata") };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);

            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.GetUserSettings(standardUser)).Returns(new UserSettingsViewModel());

            // Act
            HttpResponseMessage _response = _controller.GetUserSettings(_request);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(new UserSettingsViewModel()), _result);

        }
        #endregion

        #region SaveUserSettings
        [TestMethod]
        public void SaveUserSettings_ReturnsSaveStatus()
        {
            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = new Uri(apiAddress + "saveUserData") };
            IPortalService _portalService;
            PortalApiController _controller;

            UserSettingsViewModel userSettings = new UserSettingsViewModel();

            MockContext(out _portalService, out _controller, _request);

            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.SaveUserSettings(userSettings, standardUser)).Returns(true);

            // Act
            HttpResponseMessage _responseOK = _controller.SaveUserSettings(_request, userSettings);
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(invalidUser);
            HttpResponseMessage _responseError = _controller.SaveUserSettings(_request, userSettings);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _responseOK.StatusCode);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, _responseError.StatusCode);

        }
        #endregion

        #region GetAllFilters
        [TestMethod]
        public void GetAllFilters_ReturnsFiltersModel()
        {
            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "filters") };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);

            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.GetAllFilters(standardUser)).Returns(new AvailableFiltersModel());

            // Act
            HttpResponseMessage _response = _controller.GetAllFilters(_request);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(new AvailableFiltersModel()), _result);

        }
        #endregion

        #region GetWelcomScreen
        [TestMethod]
        public void GetWelcomeScreen_ReturnsTextModel()
        {
            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "welcomeScreen") };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);

            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.GetWelcomeScreen(standardUser)).Returns(new TextViewModel());

            // Act
            HttpResponseMessage _response = _controller.GetWelcomeScreen(_request);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(new TextViewModel()), _result);


        }
        #endregion

        #region GetSiteDetails
        [TestMethod]
        public void GetSiteDetails_ReturnsOKstatusForValidSiteForLoggedInUser()
        {
            int _testSiteId = validSites[0];
            //_request.Properties.Add(System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "sitedetails" + "/" + _testSiteId.ToString()) };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);
            SetupGetSiteDetailsForTestUser(_testSiteId, _portalService, _controller, standardUser);

            // Act
            HttpResponseMessage _response = _controller.GetSiteDetails(_request, _testSiteId);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(new SiteDetailViewModel()), _result);
        }

        [TestMethod]
        public void GetSiteDetails_RejectsInvalidSiteForLoggedInUser()
        {
            int _testSiteId = validSites[0];
            //_request.Properties.Add(System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(apiAddress + "sitedetails" + "/" + _testSiteId.ToString()) };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);
            SetupGetSiteDetailsForTestUser(_testSiteId, _portalService, _controller, invalidUser);

            // Act
            HttpResponseMessage _response = _controller.GetSiteDetails(_request, _testSiteId);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, _response.StatusCode);
        }

        private static void SetupGetSiteDetailsForTestUser(int _testSiteId, IPortalService _portalService, PortalApiController _controller, string user)
        {
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(user);
            Mock.Arrange(() => _portalService.GetSiteDetails(_testSiteId)).Returns(new SiteDetailViewModel());
            Mock.Arrange(() => _portalService.CheckUserAccess(user)).Returns(SetupUserAccess(user));
        }

        #endregion

        #region GetCostAndConsumption
        [TestMethod]
        public void GetCostAndConsumption_ReturnsModelForValidUser()
        {
            int _testSiteId = validSites[0];
            string _testFilter = "__";
            int _testMonthSpan = 12;

            // Arrange
            var _request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri =
                new Uri(apiAddress + "costAndConsumption" + "/" + _testMonthSpan.ToString() + "/" + _testFilter + "/" + _testSiteId.ToString())
            };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);
            var _chartElementView = new GoogleChartViewModel() { Columns = new List<GoogleCols>(), Rows = new List<GoogleRows>() };
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.CheckUserAccess(standardUser)).Returns(new UserAccessModel() { ValidSites = validSites });
            Mock.Arrange(() => _portalService.GetCostsAndConsumption(_testMonthSpan, new CostConsumptionOptions())).IgnoreArguments().Returns(_chartElementView);

            // Act
            HttpResponseMessage _response = _controller.GetCostsAndConsumption(_request, _testMonthSpan, _testFilter, _testSiteId);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_chartElementView), _result);
        }

        #endregion

        #region GetDashboardStatistics
        [TestMethod]
        public void GetDashboardStats_ReturnsDataForLoggedInUser()
        {
            string _testFilter = "__";
            int _testMonthSpan = 12;
            var _model = new InvoiceStatsViewModel();
            // Arrange
            var _request = new HttpRequestMessage { Method = HttpMethod.Get,
                RequestUri = new Uri(apiAddress + "dashboardStatistics" + "/" + _testMonthSpan.ToString() + "/" + _testFilter) };
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(standardUser);
            Mock.Arrange(() => _portalService.GetDashboardStatistics(standardUser, _testMonthSpan, _testFilter)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetDashboardStatistics(_request, _testMonthSpan, _testFilter);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }
        #endregion

        #region Invoices

        #region GetSiteInvoiceData
        [TestMethod]
        public void GetSiteInvoiceData_ReturnsDataForValidSite()
        {
            int _testSiteId = validSites[0];
            List<InvoiceDetail> _model = new List<InvoiceDetail>();

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("siteInvoiceDataFor" + "/" + _testSiteId.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);
            MockStandardUser(_portalService, _controller, standardUser);

            Mock.Arrange(() => _portalService.GetInvoiceDetailForSite(_testSiteId)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetSiteInvoiceData(_request, _testSiteId);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }


        #endregion

        #region GetSiteHistoricalData
        [TestMethod]
        public void GetSiteHistoricalData_ReturnsDataForValidInvoice()
        {
            int _testInvoiceId = validInvoices[0];
            List<MonthlyConsumptionViewModal> _model = new List<MonthlyConsumptionViewModal>();

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("siteHistoryDataFor" + "/" + _testInvoiceId.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller, _request);
            MockStandardUser(_portalService, _controller, standardUser);
            Mock.Arrange(() => _portalService.GetHistoricalDataForSite(_testInvoiceId)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetSiteHistoricalData(_request, _testInvoiceId);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }
        #endregion

        #region GetInvoiceOverview
        [TestMethod]
        public void GetInvoiceOverview_siteId_ReturnsDataForOverviewModel()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId;
            string _testFilter;
            List<InvoiceOverviewViewModel> _model = CreateTestEnvironmentForInvoices(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId);

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("invoiceOverviewFor" + "/" + _testSiteId.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);
            Mock.Arrange(() => _portalService.GetInvoiceOverviewForSite(_testSiteId)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetInvoiceOverview(_request, Int32.Parse(UriSegments[0]));
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }

        [TestMethod]
        public void GetInvoiceOverview_siteId_months_ReturnsDataForOverviewModel()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId;
            string _testFilter;
            List<InvoiceOverviewViewModel> _model = CreateTestEnvironmentForInvoices(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId);

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("invoiceOverviewFor" + "/" + _testSiteId.ToString() + "/" + _monthsToDisplay.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);
            Mock.Arrange(() => _portalService.GetInvoiceOverviewForSite(_testSiteId, _monthsToDisplay)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetInvoiceOverview(_request, Int32.Parse(UriSegments[0]), Int32.Parse(UriSegments[1]));
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }

        [TestMethod]
        public void GetInvoiceOverview_months_filter_page_ReturnsAllOverviewDataForUser()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId;
            string _testFilter;
            List<InvoiceOverviewViewModel> _model = CreateTestEnvironmentForInvoices(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId);

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("invoiceAllOverview" + "/" + _monthsToDisplay.ToString() + "/" + _testFilter + "/" + _pageNumber.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

            Mock.Arrange(() => _portalService.GetAllInvoiceOverview(standardUser, _monthsToDisplay, _testFilter, _pageNumber)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetInvoiceOverview(_request, Int32.Parse(UriSegments[0]), UriSegments[1], Int32.Parse(UriSegments[2]));
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }

        [TestMethod]
        public void GetInvoiceSummary_ReturnsViewModelForInvoices()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId, _testInvoiceId;
            string _testFilter;
            StandardParameters(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId, out _testInvoiceId);


            StackedBarChartViewModel _model = new StackedBarChartViewModel() { };

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("invoiceSummaryFor" + "/" + _testInvoiceId.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

            Mock.Arrange(() => _portalService.InvoiceSummaryByMonth(_testInvoiceId)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetInvoiceSummary(_request, Int32.Parse(UriSegments[0]));
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }


        [TestMethod]
        public void GetInvoiceDetail_ReturnsViewModelForInvoices()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId, _testInvoiceId;
            string _testFilter;
            StandardParameters(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId, out _testInvoiceId);


            InvoiceDetailViewModel _model = new InvoiceDetailViewModel() { };

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("invoiceDetailFor" + "/" + _testInvoiceId.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

            Mock.Arrange(() => _portalService.GetInvoiceDetail(_testInvoiceId)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetInvoiceDetail(_request, Int32.Parse(UriSegments[0]));
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }

        #endregion
        #endregion

        [TestMethod]
        public void GetSummaryDataFor_ReturnsViewModel()
        {
            SummaryViewModel _model = new SummaryViewModel() { MaxValue = 0 };

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("summarydata");
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

            Mock.Arrange(() => _portalService.GetSummaryDataFor(standardUser)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetSummaryDataFor(_request);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }

        [TestMethod]
        public void GetDetailBySite_ReturnsViewModel()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId, _testInvoiceId;
            string _testFilter;
            StandardParameters(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId, out _testInvoiceId);
            DetailBySiteViewModel _model = new DetailBySiteViewModel() { MaxTotalInvoices = 0 };

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("detailbysite" + "/" + _monthsToDisplay.ToString() + "/" + _testFilter);
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;

            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

            Mock.Arrange(() => _portalService.GetDetailBySite(standardUser, _monthsToDisplay, _testFilter, 0)).Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetDetailBySite(_request, Int32.Parse(UriSegments[0]), UriSegments[1]);
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }


        //[TestMethod]
        //public void ztest()
        //{
        //    int _monthsToDisplay, _pageNumber, _testSiteId, _testInvoiceId;
        //    string _testFilter;
        //    StandardParameters(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId, out _testInvoiceId);
        //    DetailBySiteViewModel _model = new DetailBySiteViewModel() { MaxTotalInvoices = 0 };

        //    // Arrange
        //    HttpRequestMessage _request = CreateHttpRequest("detailbysite" + "/" + _monthsToDisplay.ToString() + "/" + _testFilter);
        //    IPortalService _portalService;
        //    PortalApiController _controller;
        //    List<string> UriSegments;

        //    SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

        //    Mock.Arrange(() => _portalService.GetDetailBySite(standardUser, _monthsToDisplay, _testFilter, 0)).Returns(_model);

        //    // Act
        //    System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_request.RequestUri);
        //    httpWReq.Method = "GET";
        //    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();

        //    HttpResponseMessage _response = _controller.GetDetailBySite(_request, Int32.Parse(UriSegments[0]), UriSegments[1]);
        //    var _result = _response.Content.ReadAsStringAsync().Result;

        //    // Assert
        //    Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
        //    Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        //}



        [TestMethod]
        public void GetDatapointDetails_ReturnsDatapointIdentity()
        {
            int _monthsToDisplay, _pageNumber, _testSiteId, _testInvoiceId;
            string _testFilter;
            StandardParameters(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId, out _testInvoiceId);
            DatapointIdentity _datapointId = new DatapointIdentity() { name = "test" };
            DatapointDetailView _model = new DatapointDetailView() { };
            //CostConsumptionOptions _options = new CostConsumptionOptions() { userId = standardUser, siteId = _testSiteId, includeMissing = true };

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("DatapointDetails" + "/" + _testSiteId.ToString());
            IPortalService _portalService;
            PortalApiController _controller;
            List<string> UriSegments;
            SetupEnvironment(_request, standardUser, out _portalService, out _controller, out UriSegments);

            // Mock does not handle classes passed as parameters
            Mock.Arrange(() => _portalService.GetDatapointDetails(Arg.IsAny<DatapointIdentity>(), Arg.IsAny<CostConsumptionOptions>())).Returns(_model);
            //Mock.Arrange(() => _portalService.GetDatapointDetails(_datapointId, _options)).IgnoreArguments().Returns(_model);

            // Act
            HttpResponseMessage _response = _controller.GetDatapointDetails(_request, _datapointId, Int32.Parse(UriSegments[0]));
            var _result = _response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }
        #endregion

        #region POST requests

        [TestMethod]
        public void SetInvoiceApproved_ReturnsModelForValidUserAndInvoiceId()
        {
            // Valid invoice, valid user
            int _testInvoiceId = validInvoices[0];
            string _user = standardUser;
            // Arrange
            InvoiceOverviewViewModel _model = new InvoiceOverviewViewModel() { Approved = true };
            // Act
            HttpResponseMessage _response = ArrangeApproveInvoiceTestEnvironment(_testInvoiceId, _user, _model);
            var _result = _response.Content.ReadAsStringAsync().Result;
            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, _response.StatusCode);
            Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(_model), _result);
        }

        [TestMethod]
        public void SetInvoiceApproved_ReturnsBadRequestForInvalidInvoice()
        {
            // Invalid invoice, valid user
            int _testInvoiceId = invalidInvoices[0];
            string _user = standardUser;
            // Arrange
            InvoiceOverviewViewModel _model = new InvoiceOverviewViewModel() { Approved = true };
            // Act
            HttpResponseMessage _response = ArrangeApproveInvoiceTestEnvironment(_testInvoiceId, _user, _model);
            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, _response.StatusCode);
        }

        [TestMethod]
        public void SetInvoiceApproved_ReturnsBadRequestForInvalidUser()
        {
            // Valid invoice, invalid user
            int _testInvoiceId = validInvoices[0];
            string _user = invalidUser;
            // Arrange
            InvoiceOverviewViewModel _model = new InvoiceOverviewViewModel() { Approved = true };
            // Act
            HttpResponseMessage _response = ArrangeApproveInvoiceTestEnvironment(_testInvoiceId, _user, _model);
            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, _response.StatusCode);
        }

        [TestMethod]
        public void SetInvoiceApproved_ReturnsBadRequestIfInvoiceNotApproved()
        {
            // Valid invoice, valid user
            int _testInvoiceId = validInvoices[0];
            string _user = invalidUser;
            // Arrange
            InvoiceOverviewViewModel _model = new InvoiceOverviewViewModel() { Approved = false };
            // Act
            HttpResponseMessage _response = ArrangeApproveInvoiceTestEnvironment(_testInvoiceId, _user, _model);
            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, _response.StatusCode);
        }

        private static HttpResponseMessage ArrangeApproveInvoiceTestEnvironment(int testInvoiceId, string user, InvoiceOverviewViewModel model)
        {
            HttpRequestMessage _request;
            PortalApiController _controller;
            _request = CreateHttpRequest("invoiceApproval" + "/" + testInvoiceId.ToString(), "post");
            IPortalService _portalService;
            List<string> UriSegments;
            SetupEnvironment(_request, user, out _portalService, out _controller, out UriSegments);
            string _rootUrl = "http://localhost:2020";
            System.Web.HttpContext.Current = FakeHttpContext(_rootUrl);
            Mock.Arrange(() => _portalService.ApproveInvoice(testInvoiceId, user, _rootUrl)).Returns(model);
            return _controller.SetInvoiceApproved(_request, testInvoiceId);
        }

        #endregion
        ////[TestMethod]
        ////public void GetCompanyHierarchy_ReturnsCustomerHierachyViewModel()
        ////{
        ////    // Arrange
        ////    IPortalService _portalService;
        ////    PortalApiController _controller;
        ////    MockContext2(out _portalService, out _controller);

        ////    string _userId = "admin@cimsco.co.nz";
        ////    //  CustomerHierarchyViewModel _testData;
        ////    // string _testJsonData;
        ////    //  CreateCustomerHierachyData(out _testData, out _testJsonData);
        ////    //var MockUser = Mock.Create<System.Security.Principal.IPrincipal>();
        ////    //var MockIdentity = Mock.Create<System.Security.Principal.IIdentity>();
        ////    //Mock.Arrange(() => _controller.User).Returns(MockUser);
        ////    //Mock.Arrange(() => MockUser.Identity).Returns(MockIdentity);
        ////    //Mock.Arrange(() => MockUser.Identity.Name).Returns(_userId);
        ////    //Mock.Arrange(() => _portalService.GetSiteHierarchy(_userId)).Returns(new SiteHierarchyViewModel());

        ////    Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns("testuser");

        ////    //Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(_userId);
        ////    //  Mock.Arrange(() => _portalService.GetCompanyHierarchy(_userId)).Returns(_testData);

        ////    var _request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/sitehierarchy");
        ////    SetupControllerForTests(_controller, _request);

        ////    // Act
        ////    //var _result = _controller.GetCompanyData(_request);
        ////    var _result = _controller.GetSiteHierarchy(_request);

        ////    var _resultJsonData = _result.Content.ReadAsStringAsync().Result;// .Headers.ContentType;

        ////    // Assert
        ////    //Assert.AreEqual(_resultJsonData, _testJsonData);
        ////    Assert.IsInstanceOfType(_result.Content, typeof(SiteHierarchyViewModel));
        ////    Assert.IsInstanceOfType(_result, typeof(System.Net.Http.HttpResponseMessage));
        ////    Assert.AreEqual(_result.StatusCode, System.Net.HttpStatusCode.OK);
        ////}


        [TestMethod]
        public void GetCurrentUserAccess_ReturnsUserAccessModelForGivenUser()
        {
            string _user = standardUser;

            // Arrange
            HttpRequestMessage _request = CreateHttpRequest("null", "get");
            PortalApiController _controller;
            IPortalService _portalService;
            List<string> UriSegments;
            SetupEnvironment(_request, _user, out _portalService, out _controller, out UriSegments);

            // Act
            UserAccessModel _result = _controller.GetCurrentUserAccess();

            // Assert
            Assert.IsInstanceOfType(_result, typeof(UserAccessModel));
            Assert.AreEqual(_result.ViewInvoices, true);
            Assert.AreEqual(_result.ValidSites.Count, 3);
        }

        ////#region TestUserMethods
        ////[TestMethod]
        ////public void Test_User_1()
        ////{
        ////    // Arrange
        ////    var fakeHttpContext = Mock.Create<System.Web.HttpContextBase>();
        ////    var fakeIdentity = new System.Security.Principal.GenericIdentity("User");
        ////    var principal = new System.Security.Principal.GenericPrincipal(fakeIdentity, null);


        ////    Mock.Arrange(() => fakeHttpContext.User).Returns(principal);

        ////    var httpContext = Mock.Create<System.Web.HttpContextBase>();
        ////    //var response = Mock.Create<System.Web.HttpResponseBase>();
        ////    var routeData = new System.Web.Routing.RouteData();
        ////    ControllerContext controllerContext = new ControllerContext(httpContext, routeData, Mock.Create<ControllerBase>());
        ////    Mock.Arrange(() => httpContext.User).Returns(principal);

        ////    //Mock.Arrange(() => controllerContext.HttpContext).Returns(fakeHttpContext);

        ////    IPortalService _portalService;
        ////    PortalApiController _controller;
        ////    MockContext(out _portalService, out _controller);
        ////    // _controller.ControllerContext = controllerContext;
        ////    // Act

        ////    // Assert
        ////    Assert.AreEqual(fakeHttpContext.User.Identity.Name, "User");
        ////}

        ////[TestMethod]
        ////public void Test_User_2()
        ////{
        ////    // Arrange
        ////    IPortalService _portalService;
        ////    PortalApiController _controller;
        ////    MockContext2(out _portalService, out _controller);
        ////    Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns("test");

        ////    // Act
        ////    int zztest = 0;
        ////    zztest = _controller.ZZTest();

        ////    // Assert
        ////    Assert.AreEqual(zztest, 1);
        ////}
        ////#endregion

        #region private functions

        //private static void CreateCustomerHierachyData(out CustomerHierarchyViewModel _testData, out string _jsonData)
        //{
        //    _testData = new CustomerHierarchyViewModel()
        //    {
        //        GroupName = "Test Group Name",
        //        CustomerData = new List<CustomerData> 
        //                                            { new CustomerData { Address1 = "Addr1", CustomerName = "Customer Name", CustomerId = 1 },
        //                                              new CustomerData { Address1 = "Addr2", CustomerName = "Customer Name 2", CustomerId = 2 } }
        //    };
        //    _jsonData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(_testData);
        //}
        /// <summary>
        ///  http://timney.net/unit-testing-web-api/
        /// </summary>
        /// <param name="portalService"></param>
        /// <param name="controller"></param>

        //private static void MockContext2(out IPortalService portalService, out PortalApiController controller, HttpRequestMessage request)
        //{
        //    portalService = Mock.Create<IPortalService>();
        //    controller = Mock.Create<PortalApiController>(portalService);

        //    var config = new HttpConfiguration();
        //    var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
        //    var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });

        //    controller.ControllerContext =
        //        new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
        //    //Mock.Create<System.Web.Http.Controllers.HttpControllerContext>(config, routeData, request);
        //    controller.ControllerContext.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();
        //    controller.ControllerContext.RequestContext.Principal = Mock.Create<System.Security.Principal.IPrincipal>();
        //    controller.Request = request;
        //    controller.Configuration = config;


        //    ////controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;

        //    //System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal
        //    //(
        //    //         new System.Security.Principal.GenericIdentity("Bob", "Passport"),
        //    //        new[] {"managers", "executives"}
        //    //);


        //}

        #region Mock the standard environment 
        private static void SetupEnvironment(HttpRequestMessage _request, string UserType, out IPortalService _portalService, out PortalApiController _controller, out List<string> UriSegments)
        {
            MockContext(out _portalService, out _controller, _request);
            MockStandardUser(_portalService, _controller, UserType);
            UriSegments = ReturnUriSegments(_request);
        }

        private static void MockContext(out IPortalService portalService, out PortalApiController controller, HttpRequestMessage request)
        {
            portalService = Mock.Create<IPortalService>();
            controller = Mock.Create<PortalApiController>(portalService);

            var config = new HttpConfiguration();
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });

            controller.ControllerContext =
                new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            controller.ControllerContext.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();
            controller.ControllerContext.RequestContext.Principal = Mock.Create<System.Security.Principal.IPrincipal>();
            controller.Request = request;
            controller.Configuration = config;
        }

        private static void MockStandardUser(IPortalService _portalService, PortalApiController _controller, string user)
        {
            //            UserAccessModel _userAccessModel = new UserAccessModel() { ValidSites = new List<int>() { validSites[0] }, CanApproveInvoices = true, ViewInvoices = true };

            Mock.Arrange(() => _portalService.CheckUserAccess(user)).Returns(SetupUserAccess(user));
            Mock.Arrange(() => _portalService.CheckUserAccessToInvoice(user, validInvoices[0])).Returns(user == invalidUser ? false : true);
            Mock.Arrange(() => _portalService.CheckUserAccessToInvoice(user, invalidInvoices[0])).Returns(false);
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(user);
        }

        private static List<InvoiceOverviewViewModel> CreateTestEnvironmentForInvoices(out int _monthsToDisplay, out string _testFilter, out int _pageNumber, out int _testSiteId)
        {
            int _testInvoiceId;
            StandardParameters(out _monthsToDisplay, out _testFilter, out _pageNumber, out _testSiteId, out _testInvoiceId);
            var _model = new List<InvoiceOverviewViewModel>();
            _model.Add(new InvoiceOverviewViewModel() { Approved = true });
            return _model;
        }

        private static void StandardParameters(out int _monthsToDisplay, out string _testFilter, out int _pageNumber, out int _testSiteId, out int _testInvoiceId)
        {
            _testSiteId = validSites[0];
            _testInvoiceId = validInvoices[0];
            _monthsToDisplay = 12;
            _testFilter = "__";
            _pageNumber = 1;
        }
        #endregion

        //private static void MockContext2(out IPortalService portalService, out PortalApiController controller)
        //{
        //    portalService = Mock.Create<IPortalService>();
        //    controller = Mock.Create<PortalApiController>(portalService);

        //    var config = new HttpConfiguration();
        //    //            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/companylistfor/1");
        //    var _request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/sitehierachy");
        //    var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
        //    var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });

        //    controller.ControllerContext =
        //        new System.Web.Http.Controllers.HttpControllerContext(config, routeData, _request);
        //    //Mock.Create<System.Web.Http.Controllers.HttpControllerContext>(config, routeData, request);
        //    controller.ControllerContext.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();
        //    controller.ControllerContext.RequestContext.Principal = Mock.Create<System.Security.Principal.IPrincipal>();
        //    controller.Request = _request;
        //    controller.Configuration = config; // new HttpConfiguration();


        //    ////controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;

        //    //System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal
        //    //(
        //    //         new System.Security.Principal.GenericIdentity("Bob", "Passport"),
        //    //        new[] {"managers", "executives"}
        //    //);

        //}



        //private static void SetupControllerForTests(ApiController controller, System.Net.Http.HttpRequestMessage request)
        //{
        //    var config = new System.Web.Http.HttpConfiguration();
        //    controller.Request = request;
        //    controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;
        //}

        //private static void SetupControllerForTests2(ApiController controller)
        //{
        //    var config =
        //        //Mock.Create <System.Web.Http.HttpConfiguration>();
        //        new System.Web.Http.HttpConfiguration();
        //    var request =
        //        //Mock.Create<System.Net.Http.HttpRequestMessage>(System.Net.Http.HttpMethod.Get, "http://localhost/api/companylistfor/1");
        //        new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/companylistfor/1");
        //    var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
        //    var routeData =
        //        //Mock.Create<System.Web.Http.Routing.HttpRouteData>(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });
        //        new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });

        //    controller.ControllerContext =
        //        new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
        //    //Mock.Create<System.Web.Http.Controllers.HttpControllerContext>(config, routeData, request);

        //    controller.Request = request;
        //    controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;

        //}

        //private static void MockHttpContext(IPortalService _portalService, out PortalApiController controller, out System.Web.Http.Controllers.HttpControllerContext controllerContext)
        //{
        //    controller = new PortalApiController(_portalService);

        //    var httpContext = Mock.Create<System.Web.HttpContextBase>();
        //    var response = Mock.Create<System.Web.HttpResponseBase>();
        //    var routeData = new System.Web.Routing.RouteData();
        //    //var requestContext = Mock.Create<System.Web.Http.Controllers.HttpRequestContext>();
        //    //controllerContext = new System.Web.Http.Controllers.HttpControllerContext(httpContext, routeData, Mock.Create<ControllerBase>());

        //    //Mock.Arrange(() => httpContext.User.Identity.Name).Returns("test");
        //    //Mock.Arrange(() => httpContext.User.Identity.GetUserId()).Returns("test");
        //}

        private static HttpRequestMessage CreateHttpRequest(string url)
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(apiAddress + url)
            };
        }

        private static HttpRequestMessage CreateHttpRequest(string url, string type)
        {
            HttpRequestMessage _request = new HttpRequestMessage() { RequestUri = new Uri(apiAddress + url) };
            switch (type)
            {
                case "get":
                    _request.Method = HttpMethod.Get;
                    break;
                case "post":
                    _request.Method = HttpMethod.Get;
                    break;
            }
            return _request;
        }

        private static List<string> ReturnUriSegments(HttpRequestMessage _request)
        {
            List<string> UriSegments = new List<string>();
            for (int i = 3; i < _request.RequestUri.Segments.Length; i++)
            {
                UriSegments.Add(_request.RequestUri.Segments[i].TrimEnd('/'));
            }

            return UriSegments;
        }

        private static HttpContext FakeHttpContext(string rootUrl)
        {
            //var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var httpRequest = new HttpRequest("", rootUrl, "/");
            var stringWriter = new System.IO.StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }
        #endregion

    }
}



//[TestMethod]
//public void VerifyIdentityAndAccess_PreventsAccessToNonLinkedCompany()
//{
//    // Arrange
//    IPortalService _portalService;
//    PortalApiController _controller;
//    MockContext2(out _portalService, out _controller);
//    Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns("testuser");
//    Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.IsInRole("test")).Returns(true);
//    Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.IsAuthenticated).Returns(true);

//    Mock.Arrange(() => _portalService.ConfirmUserAccess("testuser")).Returns(true);

//    // Act
//    var result = _controller.CheckUserAccess("testuser");

//    // Assert
//    Assert.AreEqual(result, true);

//}
