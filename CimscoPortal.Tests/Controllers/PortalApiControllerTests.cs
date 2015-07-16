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

namespace CimscoPortal.Tests.Controllers
{
    [TestClass]
    public class PortalAPiControllerTests
    {

        [TestMethod]
        public void GetCompanyHierarchy_ReturnsCustomerHierachyViewModel()
        {
            // Arrange
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller);

            string _userId = "admin@cimsco.co.nz";
          //  CustomerHierarchyViewModel _testData;
           // string _testJsonData;
          //  CreateCustomerHierachyData(out _testData, out _testJsonData);

           // Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns(_userId);
          //  Mock.Arrange(() => _portalService.GetCompanyHierarchy(_userId)).Returns(_testData);

            var _request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/companyhierarchy");
            SetupControllerForTests(_controller, "http://localhost/api/companyhierarchy", _request);

            // Act
            //var _result = _controller.GetCompanyData(_request);
            var _result = _controller.GetSiteHierarchy(_request);

            var _resultJsonData = _result.Content.ReadAsStringAsync().Result;// .Headers.ContentType;

            // Assert
            //Assert.AreEqual(_resultJsonData, _testJsonData);
            Assert.IsInstanceOfType(_result, typeof(System.Net.Http.HttpResponseMessage));
            Assert.AreEqual(_result.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void VerifyIdentityAndAccess_PreventsAccessToNonLinkedCompany()
        {
            // Arrange
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext2(out _portalService, out _controller);
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns("testuser");
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.IsInRole("test")).Returns(true);
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.IsAuthenticated).Returns(true);

            Mock.Arrange(() => _portalService.ConfirmUserAccess("testuser")).Returns(true);

            // Act
            var result = _controller.CheckUserAccess("testuser");

            // Assert
            Assert.AreEqual(result, true);

        }

        [TestMethod]
        public void Test_User_1()
        {
            // Arrange
            var fakeHttpContext = Mock.Create<System.Web.HttpContextBase>();
            var fakeIdentity = new System.Security.Principal.GenericIdentity("User");
            var principal = new System.Security.Principal.GenericPrincipal(fakeIdentity, null);

            
            Mock.Arrange(() => fakeHttpContext.User).Returns(principal);
            
            var httpContext = Mock.Create<System.Web.HttpContextBase>();
            //var response = Mock.Create<System.Web.HttpResponseBase>();
            var routeData = new System.Web.Routing.RouteData();
            ControllerContext controllerContext = new ControllerContext(httpContext, routeData, Mock.Create<ControllerBase>());
            Mock.Arrange(() => httpContext.User).Returns(principal);

            //Mock.Arrange(() => controllerContext.HttpContext).Returns(fakeHttpContext);

            IPortalService _portalService;
            PortalApiController _controller;
            MockContext(out _portalService, out _controller);
            // _controller.ControllerContext = controllerContext;
            // Act

            // Assert
            Assert.AreEqual(fakeHttpContext.User.Identity.Name, "User");
        }

        [TestMethod]
        public void Test_User_2()
        {
            // Arrange
            IPortalService _portalService;
            PortalApiController _controller;
            MockContext2(out _portalService, out _controller);
            Mock.Arrange(() => _controller.ControllerContext.RequestContext.Principal.Identity.Name).Returns("test");

            // Act
            int zztest = 0;
            zztest = _controller.ZZTest(1);

            // Assert
            Assert.AreEqual(zztest, 1);
        }


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
        private static void MockContext(out IPortalService portalService, out PortalApiController controller)
        {
            portalService = Mock.Create<IPortalService>();
            controller = new PortalApiController(portalService);
            //System.Web.Http.Controllers.HttpControllerContext _controllerContext;
            //MockHttpContext(portalService, out controller, out _controllerContext);
            ////controller.ControllerContext = _controllerContext;
        }

        private static void MockContext2(out IPortalService portalService, out PortalApiController controller)
        {
            portalService = Mock.Create<IPortalService>();
            controller = Mock.Create<PortalApiController>(portalService);

            var config = new System.Web.Http.HttpConfiguration();
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/companylistfor/1");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });

            controller.ControllerContext =
                new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            //Mock.Create<System.Web.Http.Controllers.HttpControllerContext>(config, routeData, request);
            controller.ControllerContext.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();
            controller.ControllerContext.RequestContext.Principal = Mock.Create<System.Security.Principal.IPrincipal>();
            controller.Request = request;
            controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;

            //System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal
            //(
            //         new System.Security.Principal.GenericIdentity("Bob", "Passport"),
            //        new[] {"managers", "executives"}
            //);


        }

        private static void SetupControllerForTests(ApiController controller, string route, System.Net.Http.HttpRequestMessage request)
        {
            var config = new System.Web.Http.HttpConfiguration();
            controller.Request = request;
            controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        private static void SetupControllerForTests2(ApiController controller)
        {
            var config =
                //Mock.Create <System.Web.Http.HttpConfiguration>();
                new System.Web.Http.HttpConfiguration();
            var request =
                //Mock.Create<System.Net.Http.HttpRequestMessage>(System.Net.Http.HttpMethod.Get, "http://localhost/api/companylistfor/1");
                new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://localhost/api/companylistfor/1");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData =
                //Mock.Create<System.Web.Http.Routing.HttpRouteData>(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });
                new System.Web.Http.Routing.HttpRouteData(route, new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "portal" } });

            controller.ControllerContext =
                new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            //Mock.Create<System.Web.Http.Controllers.HttpControllerContext>(config, routeData, request);

            controller.Request = request;
            controller.Request.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = config;

        }

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

        #endregion
    }
}
