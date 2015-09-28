using CimscoPortal.Data;
using CimscoPortal.Models;
using CimscoPortal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Telerik.JustMock;
using Moq;
using System.Data.Entity;

namespace CimscoPortal.Tests.Services
{

    [TestClass]
    public class PortalServiceTests
    {
        Mock<ICimscoPortalContext> _repository;
        PortalService _portalService;

        [TestInitialize]
        public void Setup()
        {
            CimscoPortal.App_Start.AutoMapperConfig.Configure();
            MockContextAndService(out _repository, out _portalService);
        }

        [TestMethod]
        public void GetCommonData_ReturnsCommonInfoViewModel_IfUserValid()
        {
            // Arrange 
            var _mockSet = MockDataSet(FakeUserList());
            _repository.Setup(x => x.AspNetUsers).Returns(_mockSet);


            // Act
            var _result = _portalService.GetCommonData("testuser1");

            // Assert
            Assert.AreEqual(_result.eMail, "test1@test.com");
            Assert.IsInstanceOfType(_result, typeof(CommonInfoViewModel));
        }

        [TestMethod]
        public void GetCommonData_ReturnsError_IfUserNotValid()
        {
            // Arrange 
            var _mockSet = MockDataSet(FakeUserList());
            _repository.Setup(x => x.AspNetUsers).Returns(_mockSet);

            // Act
            var _result = _portalService.GetCommonData("nulluser");

            // Assert
            Assert.AreEqual(_result.eMail, "");
            Assert.IsInstanceOfType(_result, typeof(CommonInfoViewModel));
        }

        [TestMethod]
        public void GetNavbarDataFor_ReturnsDataForUser()
        {
            // Arrange 
            SetupMockNavbarEnvironment();

            // Act
            var _result = _portalService.GetNavbarDataFor("test1@test.com");

            // Assert
            Assert.AreEqual(_result.FirstOrDefault().Message, "Test Message1");
            Assert.AreEqual(_result.Count(), 1);
            Assert.IsInstanceOfType(_result, typeof(IEnumerable<MessageViewModel>));
        }


        [TestMethod]
        public void GetNavbarDataFor_ReturnsEmptyViewWhenNoMessages()
        {
            // Arrange 
            SetupMockNavbarEnvironment();

            // Act
            var _result = _portalService.GetNavbarDataFor("nomessages@test.com");

            // Assert
            Assert.AreEqual(_result.Count(), 0);
        }


        [TestMethod]
        public void GetSiteHierachy_ReturnsGroupDataWhenUserMemberOfGroup()
        {
            // Arrange
            SetupMockHierachyEnvironment();

            // Act
            var _result = _portalService.GetSiteHierarchy("groupuser@test.com");

            // Assert 
            Assert.IsNotNull(_result);
            Assert.AreEqual(2, _result.SiteData.Count());
            Assert.IsInstanceOfType(_result, typeof(SiteHierarchyViewModel));
        }



        [TestMethod]
        public void GetSiteHierachy_ReturnsCustomerDataWhenUserMemberOfCustomer()
        {
            // Arrange
            SetupMockHierachyEnvironment();

            // Act
            var _result = _portalService.GetSiteHierarchy("customeruser@test.com");

            // Assert 
            Assert.IsNotNull(_result);
            Assert.AreEqual(2, _result.SiteData.Count());
            Assert.IsInstanceOfType(_result, typeof(SiteHierarchyViewModel));
        }

        [TestMethod]
        public void GetSiteHierachy_ReturnsEmptyModelWhenNotMemberOfGroupOrCustomer()
        {
            // Arrange
            SetupMockHierachyEnvironment();

            // Act
            var _result = _portalService.GetSiteHierarchy("nocustomer@test.com");

            // Assert 
            Assert.IsNull(_result.HeaderName);
        }

        [TestMethod]
        public void GetSiteInvoiceData_ReturnsAllInvoicesForSite()
        {
            // Arrange
            _repository.Setup(x => x.InvoiceSummaries).Returns(MockDataSet(FakeInvoices()));

            // Act
           var _result = _portalService.GetSiteInvoiceData(1);

            // Assert
            Assert.IsNotNull(_result);
            Assert.AreEqual(1, _result.Count());
        }

        private IQueryable<Data.Models.InvoiceSummary> FakeInvoices()
        {
            List<Data.Models.Site> _sites = FakeSites();
            var _fake =
                new List<Data.Models.InvoiceSummary> 
                { 
                    new Data.Models.InvoiceSummary { AccountNumber = "123", EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "123" }, Site = _sites[0] }, 
                    new Data.Models.InvoiceSummary { AccountNumber = "234", EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "234" }, Site = _sites[0] }, 
                    new Data.Models.InvoiceSummary { AccountNumber = "345", EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "345" }, Site = _sites[1] }, 
                    new Data.Models.InvoiceSummary { AccountNumber = "456", EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "456" }, Site = _sites[1] }, 
                }.AsQueryable();
            return _fake;
        }

        # region Private functions

        private static void MockContextAndService(out Mock<ICimscoPortalContext> _repository, out PortalService _portalService)
        {
            _repository = new Mock<ICimscoPortalContext>();
            _portalService = new PortalService(_repository.Object);
        }

        private static DbSet<T> MockDataSet<T>(IQueryable<T> fake) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(fake.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(fake.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(fake.ElementType);
            // mockSet.As<IQueryable<Data.Models.PortalMessage>>().Setup(m => m.GetEnumerator()).Returns(fake.GetEnumerator());
            return mockSet.Object;
        }

        private void SetupMockNavbarEnvironment()
        {
            _repository.Setup(x => x.PortalMessages).Returns(MockDataSet(FakeMessages()));
        }

        private void SetupMockHierachyEnvironment()
        {
            _repository.Setup(x => x.Groups).Returns(MockDataSet(FakeGroupHierachy()));
            _repository.Setup(x => x.Customers).Returns(MockDataSet(FakeCustomerHierachy()));
            _repository.Setup(x => x.AspNetUsers).Returns(MockDataSet(TestUsers().AsQueryable()));
        }

        private IQueryable<Data.Models.Group> FakeGroupHierachy()
        {
            List<Data.Models.Site> _sites = new List<Data.Models.Site> 
            { 
                new Data.Models.Site { SiteName = "GroupSite1" },
                new Data.Models.Site { SiteName = "GroupSite2" }
            };

            List<Data.Models.AspNetUser> _users = TestUsers();

            var _fake = new List<Data.Models.Group>
            {
                new Data.Models.Group { GroupName = "Group1", Users = _users, Sites = _sites }
            }.AsQueryable();

            return _fake;
        }

        private IQueryable<Data.Models.Customer> FakeCustomerHierachy()
        {
            List<Data.Models.Site> _sites = FakeSites();

            List<Data.Models.AspNetUser> _users = TestUsers();

            var _fake = new List<Data.Models.Customer>
            {
                new Data.Models.Customer { CustomerName = "Customer1", Users = _users, Sites = _sites }
            }.AsQueryable();

            return _fake;
        }

        private static List<Data.Models.Site> FakeSites()
        {
            List<Data.Models.Site> _sites = new List<Data.Models.Site> 
            { 
                new Data.Models.Site { SiteId = 1, SiteName = "CustomerSite1" },
                new Data.Models.Site { SiteId = 2, SiteName = "CustomerSite2" }
            };
            return _sites;
        }


        private IQueryable<Data.Models.PortalMessage> FakeMessages()
        {
            var _dummyMessageFormat = new Data.Models.MessageFormat { MessageType = new Data.Models.MessageType { PageElement = "test" } };
            var _testUserList = TestUsers();

            var _fake = new List<Data.Models.PortalMessage> 
            {
            new Data.Models.PortalMessage { User = _testUserList[0], ExpiryDate = DateTime.Now, Message = "Test Message1", MessageFormat = _dummyMessageFormat },
            new Data.Models.PortalMessage { User = _testUserList[1], ExpiryDate = DateTime.Now, Message = "Test Message2", MessageFormat = _dummyMessageFormat },
            new Data.Models.PortalMessage { User = _testUserList[2], ExpiryDate = DateTime.Now, Message = "Test Message3", MessageFormat = _dummyMessageFormat }         
            }.AsQueryable(); // AsEnumerable();
            return _fake;
        }

        private static IQueryable<Data.Models.AspNetUser> FakeUserList()
        {
            return TestUsers().AsQueryable();
        }

        private static List<Data.Models.AspNetUser> TestUsers()
        {
            var _customer = new List<Data.Models.Customer> { new Data.Models.Customer { CustomerName = "Test Customer" } };
            var _group = new List<Data.Models.Group> { new Data.Models.Group { GroupName = "Test Group" } };

            return new List<Data.Models.AspNetUser> 
            {
            new Data.Models.AspNetUser { Email ="test1@test.com", UserName="testuser1"},
            new Data.Models.AspNetUser { Email ="test2@test.com", UserName="testuser2"},
            new Data.Models.AspNetUser { Email ="customeruser@test.com", UserName="testuser3", Customers = _customer},
            new Data.Models.AspNetUser { Email ="groupuser@test.com", UserName="testuser4", Groups = _group},                    
            new Data.Models.AspNetUser { Email ="nocustomer@test.com", UserName="testuser5", Customers = new List<Data.Models.Customer>() }                    
            };
        }

        #endregion
    }
}


//private static DbSet<T> MockDbSet<T>(List<T> table) where T : class
//{
//    var dbSet = new Mock<DbSet<T>>();
//    dbSet.As<IQueryable<T>>().Setup(q => q.Provider).Returns(() => table.AsQueryable().Provider);
//    dbSet.As<IQueryable<T>>().Setup(q => q.Expression).Returns(() => table.AsQueryable().Expression);
//    dbSet.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(() => table.AsQueryable().ElementType);
//    dbSet.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(() => table.AsQueryable().GetEnumerator());
//    dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>(table.Add);
//    dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(table.AddRange);
//    dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>(t => table.Remove(t));
//    dbSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts =>
//    {
//        foreach (var t in ts) { table.Remove(t); }
//    });
//    return dbSet.Object;
//}

//private static Mock<DbSet<Data.Models.AspNetUser>> MockUserSetup(IQueryable<Data.Models.AspNetUser> fake)
//{
//    var mockSet = new Mock<System.Data.Entity.DbSet<Data.Models.AspNetUser>>();
//    mockSet.As<IQueryable<Data.Models.AspNetUser>>().Setup(m => m.Provider).Returns(fake.Provider);
//    mockSet.As<IQueryable<Data.Models.AspNetUser>>().Setup(m => m.Expression).Returns(fake.Expression);
//    mockSet.As<IQueryable<Data.Models.AspNetUser>>().Setup(m => m.ElementType).Returns(fake.ElementType);
//    mockSet.As<IQueryable<Data.Models.AspNetUser>>().Setup(m => m.GetEnumerator()).Returns(fake.GetEnumerator());
//    return mockSet;
//}

