using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CimscoPortal;
using CimscoPortal.Controllers;
using Telerik.JustMock;

namespace CimscoPortal.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexRedirectsToPortalIndexIfUserAuthenticated()
        {
            // Arrange
            HomeController controller = new HomeController();
            var httpContext = Mock.Create<System.Web.HttpContextBase>();
            var response = Mock.Create<System.Web.HttpResponseBase>();
            var routeData = new System.Web.Routing.RouteData();
            var controllerContext = new ControllerContext(httpContext, routeData, Mock.Create<ControllerBase>());
            Mock.Arrange(() => httpContext.Request.IsAuthenticated).Returns(true);
            controller.ControllerContext = controllerContext;

            // Act
            //ViewResult result = controller.Index() as ViewResult;
            RedirectToRouteResult result = controller.Index() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
