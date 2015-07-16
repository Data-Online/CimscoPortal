using CimscoPortal.Data;
using CimscoPortal.Models;
using CimscoPortal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;

namespace CimscoPortal.Tests.Services
{
    [TestClass]
    public class PortalServiceTests
    {
        [TestMethod]
        public void GetCompanyDataFor_ReturnsHierachyForCustomer()
        {
            //// Arrange 
            //ICimscoPortalContext _repository;
            //PortalService _portalService;
            //MockContext(out _repository, out _portalService);
            //Mock.Arrange(() => _repository.Contacts.Where(i => i.ContactId == 1).Select.Returns(
            //    new CustomerHierarchyViewModel()
            //              {
            //    GroupName = "Test Group Name",
            //    CustomerData = new List<CustomerData> 
            //                                        { new CustomerData { Address1 = "Addr1", CustomerName = "Customer Name", CustomerId = 1 },
            //                                          new CustomerData { Address1 = "Addr2", CustomerName = "Customer Name 2", CustomerId = 2 } }
            //});

            //// Act
            //var _result = _portalService.GetCompanyData(1);
            //var _testData = new CustomerHierarchyViewModel();
            //// Assert
            //Assert.AreEqual(_testData.ToString(), _result.ToString());

        }

        private static void MockContext(out ICimscoPortalContext _repository, out PortalService _portalService)
        {
            _repository = Mock.Create<ICimscoPortalContext>();
            _portalService = new PortalService(_repository);
        }

    }
}
