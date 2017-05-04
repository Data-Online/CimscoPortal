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

            var _mockInvoiceSet = MockDataSet(FakeInvoices());
            var _mockSiteSet = MockDataSet(FakeSites());
            _repository.Setup(x => x.InvoiceSummaries).Returns(_mockInvoiceSet);
            _repository.Setup(x => x.Sites).Returns(_mockSiteSet);

            var _mockUserSet = MockDataSet(FakeUserList());
            _repository.Setup(x => x.AspNetUsers).Returns(_mockUserSet);

            //Mock<ComparisonBarChart> _barChart = new Mock<ComparisonBarChart>();
            //_barChart.Setup(x => x.GetBarMinMaxValues()).Returns(new List<double>() { 2.3, 2.4, 2.5 });

            //Mock<PortalService> _svc = new Mock<PortalService>();
            //_svc.Setup(x => x.GetSitesBasedOnOptions)
        }

        #region Active test

        #region Common data and tools
        [TestMethod]
        public void GetCommonData_ReturnsCommonInfoViewModel_IfUserValid()
        {
            // Arrange 
            //var _mockSet = MockDataSet(FakeUserList());
            //_repository.Setup(x => x.AspNetUsers).Returns(_mockSet);

            // Act
            var _result = _portalService.GetCommonData("customeruser@test.com");

            // Assert
            Assert.AreEqual("customeruser@test.com", _result.eMail);
            Assert.AreEqual("Test Customer", _result.TopLevelName);
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
            Assert.IsNull(_result.eMail);
            //Assert.IsNull(_result);
            Assert.IsInstanceOfType(_result, typeof(CommonInfoViewModel));
        }

        [TestMethod]
        public void GetComparisonData_ReturnsValidModel()
        {
            //var _mockInvoiceSet = MockDataSet(FakeInvoices());
            //var _mockSiteSet = MockDataSet(FakeSites());

            //_repository.Setup(x => x.InvoiceSummaries).Returns(_mockInvoiceSet);
            //_repository.Setup(x => x.Sites).Returns(_mockSiteSet);
            var _result = _portalService.GetComparisonData(12, new CostConsumptionOptions { filter = "__", siteId = 0, userId = "groupuser@test.com" });

            Assert.IsInstanceOfType(_result, typeof(GoogleChartViewModel));
        }

        #endregion

        #endregion Active tests

        [TestMethod]
        public void CheckUserAccess_ReturnsValidModel()
        {
            // Arrange
            ////var _mockSet = MockDataSet(FakeSiteList());
            ////_repository.Setup(x => x.Sites).Returns(_mockSet);
            var zz = new PrivateObject(_portalService);
            var output = (List<int>)zz.Invoke("GetValidSiteIdListForUser", "test");
        }

        [TestMethod]
        public void GetNavbarDataFor_ReturnsDataForUser()
        {
            // Arrange 
            SetupMockNavbarEnvironment();

            // Act
            var _result = _portalService.GetNavbarData("test1@test.com");

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
            var _result = _portalService.GetNavbarData("nomessages@test.com");

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
            Assert.IsNull(_result.TopLevelName);
        }

        [TestMethod]
        public void GetSiteInvoiceData_ReturnsAllInvoicesForSite()
        {
            // Arrange
            _repository.Setup(x => x.InvoiceSummaries).Returns(MockDataSet(FakeInvoices()));

            // Act
            var _result = _portalService.GetInvoiceDetailForSite(1);

            // Assert
            Assert.IsNotNull(_result);
            Assert.AreEqual(9, _result.Count());
        }

        private IQueryable<Data.Models.InvoiceSummary> FakeInvoices()
        {
            List<Data.Models.Site> _sites = FakeSites().ToList();
            var _fake =
                new List<Data.Models.InvoiceSummary>
                {
                   new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=11315.04M, GstTotal=1475.88M,KwhTotal=69387.7M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("02/05/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-6870",InvoiceTotal=13398.8M, GstTotal=1747.67M,KwhTotal=81396.98M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/06/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=16273.69M, GstTotal=2122.66M,KwhTotal=98050.44M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/07/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=17127.97M, GstTotal=2234.09M,KwhTotal=102245.16M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/08/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=16976.62M, GstTotal=2214.35M,KwhTotal=107883.22M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/09/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=13698.89M, GstTotal=1786.81M,KwhTotal=86627.92M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("03/10/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=10185.56M, GstTotal=1328.56M,KwhTotal=74117.14M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/11/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=12609.27M, GstTotal=1644.7M,KwhTotal=93351.8M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/12/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=13524.29M, GstTotal=1764.05M,KwhTotal=107680.6M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("04/01/2017")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-687",InvoiceTotal=15088.02M, GstTotal=1968.01M,KwhTotal=112809.46M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0000" }, SiteId = _sites[0].SiteId,Site = _sites[0],InvoiceDate=Convert.ToDateTime("01/02/2017")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=7043.65M, GstTotal=918.74M,KwhTotal=43117.9M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("02/05/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=8903.83M, GstTotal=1161.37M,KwhTotal=51362.5M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/06/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=8271.54M, GstTotal=1078.9M,KwhTotal=45702.99M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/07/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=8885.96M, GstTotal=1159.04M,KwhTotal=49363.79M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/08/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=8372.86M, GstTotal=1092.12M,KwhTotal=47520.92M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/09/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=4513.66M, GstTotal=588.74M,KwhTotal=28464.17M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/11/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=4598.14M, GstTotal=599.76M,KwhTotal=30152.73M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/12/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=5493.02M, GstTotal=716.48M,KwhTotal=38010.51M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("04/01/2017")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-660",InvoiceTotal=8618.03M, GstTotal=1124.09M,KwhTotal=59199.78M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, SiteId = _sites[1].SiteId,Site = _sites[1],InvoiceDate=Convert.ToDateTime("01/02/2017")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=6843.4M, GstTotal=892.62M,KwhTotal=49758.936M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("02/05/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=7914.14M, GstTotal=1032.28M,KwhTotal=53298.672M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("01/06/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=9426.02M, GstTotal=1229.48M,KwhTotal=62466.624M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("01/08/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=8971.21M, GstTotal=1170.16M,KwhTotal=61349.92M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("01/09/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=7703.67M, GstTotal=1004.83M,KwhTotal=55167.792M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("03/10/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=6350.69M, GstTotal=828.35M,KwhTotal=50867.352M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("01/11/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=5924.61M, GstTotal=772.78M,KwhTotal=49801.416M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("01/12/2016")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=5724.28M, GstTotal=746.65M,KwhTotal=49918.752M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("04/01/2017")},
                    new Data.Models.InvoiceSummary { AccountNumber="160-003-679",InvoiceTotal=6346.12M, GstTotal=827.76M,KwhTotal=51147.84M,EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, SiteId = _sites[2].SiteId,Site = _sites[2],InvoiceDate=Convert.ToDateTime("01/02/2017")}




                    //new Data.Models.InvoiceSummary { AccountNumber = "123",InvoiceTotal = 11315.04M, GstTotal = (1475.88M), KwhTotal = 69387.7M,
                    //    EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, Site = _sites[0] }, 
                    //new Data.Models.InvoiceSummary { AccountNumber = "123", 
                    //    InvoiceTotal = 13398.8M, GstTotal = (1747.67M), KwhTotal = 81396.98M,
                    //    EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0001" }, Site = _sites[0] }, 
                    //new Data.Models.InvoiceSummary { AccountNumber = "234", 
                    //    InvoiceTotal = 11315.04M, GstTotal = (1475.88M), KwhTotal = 69387.7M,
                    //    EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, Site = _sites[1] }, 
                    //new Data.Models.InvoiceSummary { AccountNumber = "234",
                    //    InvoiceTotal = 11315.04M, GstTotal = (1475.88M), KwhTotal = 69387.7M,
                    //    EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0002" }, Site = _sites[1] },
                    //new Data.Models.InvoiceSummary { AccountNumber = "345",
                    //    InvoiceTotal = 11315.04M, GstTotal = (1475.88M), KwhTotal = 69387.7M,
                    //    EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0003" }, Site = _sites[2] },
                    //new Data.Models.InvoiceSummary { AccountNumber = "345",
                    //    InvoiceTotal = 11315.04M, GstTotal = (1475.88M), KwhTotal = 69387.7M,
                    //    EnergyPoint = new Data.Models.EnergyPoint { EnergyPointNumber = "EP0003" }, Site = _sites[2] }
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
            List<Data.Models.Site> _sites = FakeSites().ToList();

            List<Data.Models.AspNetUser> _users = TestUsers();

            var _fake = new List<Data.Models.Customer>
            {
                new Data.Models.Customer { CustomerName = "Customer1", Users = _users, Sites = _sites }
            }.AsQueryable();

            return _fake;
        }

        //private static List<Data.Models.Site> FakeSites()
        //{
        //    List<Data.Models.Site> _sites = new List<Data.Models.Site> 
        //    { 
        //        new Data.Models.Site { SiteId = 1, SiteName = "CustomerSite1" },
        //        new Data.Models.Site { SiteId = 2, SiteName = "CustomerSite2" }
        //    };
        //    return _sites;
        //}

        private IQueryable<Data.Models.Site> FakeSites()
        {
            var _sites = new List<Data.Models.Site>
            {
                new Data.Models.Site { SiteId = 1, SiteName = "CustomerSite1", TotalFloorSpaceSqMeters = 800 },
                new Data.Models.Site { SiteId = 2, SiteName = "CustomerSite2", TotalFloorSpaceSqMeters = 1000 },
                new Data.Models.Site { SiteId = 3, SiteName = "CustomerSite3", TotalFloorSpaceSqMeters = 1200 }
            }.AsQueryable();

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
            var _customer = new List<Data.Models.Customer> {
                new Data.Models.Customer { CustomerName = "Test Customer" }
            };
            var _group = new List<Data.Models.Group> {
                new Data.Models.Group { GroupName = "Test Group" }
            };
            var _emptyCustomerList = new List<Data.Models.Customer>();
            var _emptyGroupList = new List<Data.Models.Group>();

            return new List<Data.Models.AspNetUser>
            {
                new Data.Models.AspNetUser { Email ="customeruser@test.com", UserName="customeruser@test.com", Customers = _customer, Groups = _emptyGroupList },
                new Data.Models.AspNetUser { Email ="groupuser@test.com", UserName="groupuser@test.com", Groups = _group, Customers = _emptyCustomerList},
                new Data.Models.AspNetUser { Email ="customeruser@test.com", UserName="testuser3", Customers = _customer},
                new Data.Models.AspNetUser { Email ="groupuser@test.com", UserName="testuser4", Groups = _group},
                new Data.Models.AspNetUser { Email ="nocustomer@test.com", UserName="testuser5", Customers = _emptyCustomerList },
                new Data.Models.AspNetUser { Email ="nogroup@test.com", UserName="testuser6", Groups = _emptyGroupList }
            };
        }

        private static IQueryable<Data.Models.Site> FakeSiteList()
        {
            return TestSites().AsQueryable();
        }

        private static List<Data.Models.Site> TestSites()
        {
            return new List<Data.Models.Site>
            {
                new Data.Models.Site { SiteId = 1, SiteName = "TestSite1" },
                new Data.Models.Site { SiteId = 2, SiteName = "TestSite2" }
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

