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

namespace CimscoPortal.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTests
    {
        [TestMethod]
        public void PortalIndexReturnsCorrectModel()
        {
            // Arrange
            var _portalService = Mock.Create<IPortalService>();
            PortalController controller = new PortalController(_portalService);

            //// Act
            ViewResult result = controller.Index() as ViewResult;

            //// Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNavbarDataReturnsValidData()
        {
            // Arrange
            var _portalService = Mock.Create<IPortalService>();
            PortalController controller = new PortalController(_portalService);
            
            int customerId = 1;
            string elementType = "pg-alert";
            Mock.Arrange(() => _portalService.GetNavbarDataFor(customerId, elementType))
                .Returns(new List<AlertViewModel>()
                 { 
                    new AlertViewModel { CategoryName = "alert_tick_red", TypeName = "Alert" }
                 })
                .MustBeCalled();

            /// Act
            JsonResult result = controller.GetNavbarData("Alert") as JsonResult;
            //var model = result as JsonResult;
           // List<string> data = result.Data as List<string>;
            /// Assert
            //var zz = result.Data.GetType().GetProperty("MessageCategory");
         //   Assert.AreEqual(1, data.Count);
            Assert.IsNotNull(result);
            //Assert.IsNotNull(result.Data);
            //Assert.AreEqual("alert_tick_red", zz);
            //Assert.IsTrue(result.ToString().Contains("alert_tick_red"));
           // Assert.AreEqual(25, model.ToString().Length);


        }
    }

}
