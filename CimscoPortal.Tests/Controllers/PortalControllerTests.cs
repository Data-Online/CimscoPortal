using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Telerik.JustMock;
using CimscoPortal.Controllers;
using CimscoPortal.Infrastructure;
using CimscoPortal.Data;
using CimscoPortal.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;

using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;

namespace CimscoPortal.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTests
    {
        [TestMethod]
        public void IndexReturnsViewModel()
        {
            // Arrange
            IPortalService _portalService;
            PortalController _controller;
            MockContext(out _portalService, out _controller);

            //// Act
            ViewResult _result = _controller.Index() as ViewResult;

            //// Assert
            Assert.IsNotNull(_result);
        }

        [TestMethod]
        public void SiteSummaryReturnsViewModel()
        {
            // Arrange
            IPortalService _portalService;
            PortalController _controller;
            MockContext(out _portalService, out _controller);

            //// Act
            ViewResult _result = _controller.SiteSummary("id") as ViewResult;

            //// Assert
            Assert.IsNotNull(_result);
        }

        [TestMethod]
        public void GetNavbarDataReturnsJsonResultForCustomer()
        {
            // Arrange
            IPortalService _portalService;
            PortalController _controller;
            MockContext(out _portalService, out _controller);

            int _customerId = 1;
            string _elementType = "pg-alert";
           
            Mock.Arrange(() => _portalService.GetNavbarDataFor(_customerId, _elementType))
                .Returns(new List<AlertData>()
                 { 
                    new AlertData { CategoryName = "alert_tick_red", TypeName = "Alert", _timeStamp = new System.DateTime(2015,1,1) }
                 })
                .MustBeCalled();

            /// Act
            JsonResult _result = _controller.GetNavbarData(_elementType) as JsonResult;
            dynamic data = _result.Data;

    

            /// Assert
            Assert.AreEqual("pg-alert", data.HeaderData.DataFor);
            Assert.AreEqual("1 January", data.AlertData[0].TimeStamp);
  
        }

        [TestMethod]
        public void DonutChartDataReturnsJsonResult()
        { 
            // Arrange
            IPortalService _portalService;
            PortalController _controller;
            MockContext(out _portalService, out _controller);

            string _elementType = "test";

            Mock.Arrange(() => _portalService.GetCurrentMonth(2))
                .Returns(new DonutChartViewModel()
                {
                    DonutChartData = new List<DonutChartData> { new DonutChartData { Label = "test", Value = 10.0M } },
                    HeaderData = new HeaderData() { Header = "test" },
                    SummaryData = new List<SummaryData>() { }
                });
            // Act
            JsonResult _result = _controller.DonutChartData(_elementType) as JsonResult;
            dynamic data = _result.Data;

            // Assert
            Assert.IsNotNull(_result);
            Assert.AreEqual("MonthlySummary", data.HeaderData.DataFor);
        }

        #region snippets
        //var _serializer = new JavaScriptSerializer();
        //var _output = _serializer.Serialize(_result.Data);
        //Assert.AreEqual(271, _output.Length);
        //Assert.IsTrue(_output.IndexOf("pg-alert") > 0);
        // Assert.IsNotNull(_result);
        #endregion

        #region Private functions

        private static void MockHttpContext(IPortalService _portalService, out PortalController controller, out ControllerContext controllerContext)
        {
            controller = new PortalController(_portalService);

            var httpContext = Mock.Create<System.Web.HttpContextBase>();
            var response = Mock.Create<System.Web.HttpResponseBase>();
            var routeData = new System.Web.Routing.RouteData();
            controllerContext = new ControllerContext(httpContext, routeData, Mock.Create<ControllerBase>());

            // Mock.Arrange(() => httpContext.User.Identity.Name).Returns("test");
            //Mock.Arrange(() => httpContext.User.Identity.GetUserId()).Returns("test");
        }

        private static void MockContext(out IPortalService _portalService, out PortalController _controller)
        {
            _portalService = Mock.Create<IPortalService>();
            _controller = new PortalController(_portalService);
            ControllerContext _controllerContext;
            MockHttpContext(_portalService, out _controller, out _controllerContext);
            _controller.ControllerContext = _controllerContext;
        }
        #endregion
    }

}
