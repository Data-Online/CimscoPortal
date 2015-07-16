namespace CimscoPortal.Data.Migrations
{
    using CimscoPortal.Data.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CimscoPortal.Data.Models.CimscoPortalContext>
    {

        private string[] _sampleUsers = new string[] { "user1@cimsco.co.nz", "user2@cimsco.co.nz", "user3@cimsco.co.nz", "user4@cimsco.co.nz", "admin@cimsco.co.nz" };
        private string[] _sampleCustomers = new string[] { "Test Customer 1", "Test Customer 2", "Test Customer 3" };
        private string[] _sampleSites = new string[] { "Site 1", "Site 2", "Site 3", "Site 4", "Site 5", "Site 6" };
        private string[] _sampleGroups = new string[] { "Test Group 1", "Test Group 2", "Test Group 3", "Test Group 4" };

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            // GPA -- Refactor!!

            RemoveGroups(context);
            RemoveCustomers(context);
            context.SaveChanges();

            CreateSites(context);
            CreateGroupsAndCustomers(context);
            context.SaveChanges();
            LinkSitesToCustomers(context);
            LinkUsersToCustomers(context);
            //TempSeed(context);
            context.SaveChanges();
            int _monthsOfDataToCreate = 36;
            //string _setAddprovedBy = "user4@cimsco.co.nz";
            context.Database.ExecuteSqlCommand("delete from [InvoiceSummaries]");
            // Commented out to ensure all data is populated correctly
            InvoiceDataSeed(context);
            InvoiceDataSeed(context, "Site 1", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Pak 'n Save Upper Hutt", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Pak 'n Save Upper Hutt Fuel Site..", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Pak .n Save Upper Hutt Bulk warehouse", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "HP Lane St Data Center", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Mega Mitre 10 Petone", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Intercontinental Wellington", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Mega Mitre 10 Porirua", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Mega Retail Park Upper Hutt", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Mega Mitre 10 Upper Hutt", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Mitre 10 Mega - Nelson", _monthsOfDataToCreate);
            InvoiceDataSeed(context, "Holiday Inn", _monthsOfDataToCreate);

            InvoiceDataSeedActual(context);
            //  InvoiceDataSeed(context, "Site 6", "01-01-2013", 24);

            context.SaveChanges();

            CreateTestMessages(context);

            CreateContacts(context);

            context.Cities.AddOrUpdate(
                cd => cd.CityName,
                new CimscoPortal.Data.Models.City { CityName = "Dunedin" },
                new CimscoPortal.Data.Models.City { CityName = "Wellington" },
                new CimscoPortal.Data.Models.City { CityName = "Christchurch" },
                new CimscoPortal.Data.Models.City { CityName = "Auckland" }
                );

            ApplyNames(context);

            context.Database.ExecuteSqlCommand("update [AspNetUsers] set [FirstName] = 'John', [LastName] = 'Doe', [CompanyLogo] = 'PakNSave.jpg' where [eMail] = 'user4@cimsco.co.nz'");
            context.Database.ExecuteSqlCommand("update [AspNetUsers] set [FirstName] = 'Cimsco', [LastName] = 'Admin', [CompanyLogo] = 'uhf_ic_logo.png' where [eMail] = 'admin@cimsco.co.nz'");

        }

        private void ApplyNames(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            int _count = 1;
            foreach (string _userId in _sampleUsers)
            {
                AddNamesToUserId(context, _userId, "Firstname" + _count.ToString(), "Lastname" + _count.ToString());
                _count++;
            }
        }

        private void AddNamesToUserId(CimscoPortal.Data.Models.CimscoPortalContext context, string userName, string firstName, string lastName)
        {
            AspNetUser _user = new AspNetUser();
            UserDetail _userDetail = new UserDetail();
            // _approver = context.Sites.Where(s => s.SiteId == _siteId).FirstOrDefault().Customer.Users.FirstOrDefault();
            _user = context.AspNetUsers.Where(s => s.Email == userName).FirstOrDefault();
            if (_user != null)
            {
                //    _user.UserDetail = new UserDetail() { FirstName = firstName, LastName = lastName };
                //_userDetail = new UserDetail() { FirstName = firstName, LastName = lastName, UserId = _user };
                //context.AspNetUsers.Add(_userDetail);
            }
        }

        private void RemoveGroups(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            IQueryable<Models.Site> _sitesToRemove;
            Group _groupToRemove;
            foreach (string _groupName in _sampleGroups)
            {
                _sitesToRemove = context.Sites.Where(s => s.Group.GroupName == _groupName);

                foreach (var _site in _sitesToRemove)
                {
                    context.Sites.Remove(_site);
                }
                _groupToRemove = context.Groups.Where(s => s.GroupName == _groupName).FirstOrDefault();
                if (_groupToRemove != null)
                {
                    context.Groups.Remove(_groupToRemove);
                }
            }
        }

        private void RemoveCustomers(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            IQueryable<Models.Site> _sitesToRemove;
            //string[] _targetSites = { _sampleCustomers[0] }; //"Test Customer 1", "Test Customer 2" };
            CimscoPortal.Data.Models.Customer _customerToRemove;
            foreach (string _customerName in _sampleCustomers)
            {
                _sitesToRemove = context.Sites.Where(s => s.Customer.CustomerName == _customerName);

                foreach (var _site in _sitesToRemove)
                {
                    context.Sites.Remove(_site);
                }
                _customerToRemove = context.Customers.Where(s => s.CustomerName == _customerName).FirstOrDefault();
                if (_customerToRemove != null)
                {
                    context.Customers.Remove(_customerToRemove);
                }
            }
            //return _sitesToRemove;
        }

        #region private functions
        private static void CreateContacts(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            context.Contacts.AddOrUpdate(
                cd => cd.LastName,
                new Contact
                {
                    FirstName = "Stephen",
                    LastName = "Nelson",
                    eMail = "StephenN@megapetone.co.nz"
                }
            );
        }

        private static void CreateTestMessages(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            context.MessageTypes.AddOrUpdate(
                cd => cd.Description,
                    new MessageType { MessageTypeId = 1, Description = "Alert", PageElement = "pg-alert" },
                    new MessageType { MessageTypeId = 2, Description = "Note", PageElement = "pg-note" },
                    new MessageType { MessageTypeId = 3, Description = "ToDo", PageElement = "pg-todo" }
                    );

            context.MessageFormats.AddOrUpdate(
                cd => cd.DisplayFormat,
                    new MessageFormat
                    {
                        MessageTypeId = 1,
                        Element1 = "fa fa-phone bg-themeprimary white",
                        Element2 = "fa fa-clock-o themeprimary",
                        DisplayFormat = "Phone Blue"
                    },
                    new MessageFormat
                    {
                        MessageTypeId = 1,
                        Element1 = "fa fa-check bg-darkorange white",
                        Element2 = "fa fa-clock-o darkorange",
                        DisplayFormat = "Tick Red"
                    },
                    new MessageFormat
                    {
                        MessageTypeId = 1,
                        Element1 = "fa fa-gift bg-warning white",
                        Element2 = "fa fa-gift bg-warning white",
                        DisplayFormat = "Event Orange"
                    }
                    );

            context.PortalMessages.AddOrUpdate(
                cd => cd.Message,
                    new PortalMessage { CustomerId = 3, Message = "Test Message 1", MessageFormatId = 3, TimeStamp = DateTime.Now, ExpiryDate = DateTime.Now })
                    ;
        }

        private void LinkSitesToCustomers(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            string[] _targets;
            string _targetCustomer;

            _targetCustomer = _sampleCustomers[0];
            _targets = new string[] { _sampleSites[0], _sampleSites[1] };
            CreateSiteCustomerLinks(_targetCustomer, _targets, context);

            _targetCustomer = _sampleCustomers[1];
            _targets = new string[] { _sampleSites[2], _sampleSites[3] };
            CreateSiteCustomerLinks(_targetCustomer, _targets, context);

            _targetCustomer = _sampleCustomers[2];
            _targets = new string[] { _sampleSites[4], _sampleSites[5] };
            CreateSiteCustomerLinks(_targetCustomer, _targets, context);

            //
            // ==> Script name : link_customers_to_sites_from_source.sql
            _targetCustomer = "Customer not set"; _targets = new string[] { "Site not set" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Masterton Supermarkets Ltd"; _targets = new string[] { "Pak 'n Save Upper Hutt" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Masterton Supermarkets Ltd"; _targets = new string[] { "Pak 'n Save Upper Hutt Fuel Site.." }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Customer not set"; _targets = new string[] { "HP Lane St Data Center" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Nees Hardware Ltd"; _targets = new string[] { "Mega Mitre 10 Petone" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Intercontinental Group"; _targets = new string[] { "Intercontinental Wellington" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Masterton Supermarkets Ltd"; _targets = new string[] { "Pak .n Save Upper Hutt Bulk warehouse" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Nees Hardware Ltd"; _targets = new string[] { "Mega Mitre 10 Porirua" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Nees Hardware Ltd"; _targets = new string[] { "Mega Retail Park Upper Hutt" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Nees Hardware Ltd"; _targets = new string[] { "Mega Mitre 10 Upper Hutt" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Field Nelson Holdings Ltd & Nelson Mega Ltd"; _targets = new string[] { "Mitre 10 Mega - Nelson" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);
            _targetCustomer = "Customer not set"; _targets = new string[] { "Holiday Inn" }; CreateSiteCustomerLinks(_targetCustomer, _targets, context);

        }

        private void CreateSiteCustomerLinks(string targetCustomer, string[] targetSites, CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            var _siteToAllocate = new Site();
            CimscoPortal.Data.Models.Customer _cust = context.Customers.Where(x => x.CustomerName == targetCustomer).First();
            foreach (string _entry in targetSites)
            {
                _siteToAllocate = context.Sites.Where(s => s.SiteName == _entry).FirstOrDefault();
                _cust.Sites.Add(_siteToAllocate);
            }
        }

        private void LinkUsersToCustomers(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            string _targetCustomer;
            string[] _targets;

            _targetCustomer = _sampleCustomers[0];
            _targets = new string[] { _sampleUsers[0], _sampleUsers[1] };
            LinkUsersCustomer(_targetCustomer, _targets, context);

            _targetCustomer = _sampleCustomers[1];
            _targets = new string[] { _sampleUsers[2] };
            LinkUsersCustomer(_targetCustomer, _targets, context);

            _targetCustomer = "Masterton Supermarkets Ltd";
            _targets = new string[] { _sampleUsers[3] };
            LinkUsersCustomer(_targetCustomer, _targets, context);

            _targetCustomer = "Intercontinental Group";
            _targets = new string[] { _sampleUsers[4] };
            LinkUsersCustomer(_targetCustomer, _targets, context);
        }

        private void LinkUsersCustomer(string targetCustomer, string[] targetUsers, CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            AspNetUser[] _userList;
            CimscoPortal.Data.Models.Customer _cust = context.Customers.Where(x => x.CustomerName == targetCustomer).First();
            foreach (string _entry in targetUsers)
            {
                _userList = context.AspNetUsers.Where(x => x.UserName == _entry).ToArray();
                foreach (var _userId in _userList)
                {
                    _cust.Users.Add(_userId);
                }
            }
        }

        private void CreateSites(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            // Sites
            // ==> Script Name : seed_Sites_table_from_source.sql
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Site not set" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Pak 'n Save Upper Hutt" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Pak 'n Save Upper Hutt Fuel Site.." });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "HP Lane St Data Center" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Mega Mitre 10 Petone" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Intercontinental Wellington" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Pak .n Save Upper Hutt Bulk warehouse" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Mega Mitre 10 Porirua" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Mega Retail Park Upper Hutt" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Mega Mitre 10 Upper Hutt" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Mitre 10 Mega - Nelson" });
            context.Sites.AddOrUpdate(cd => cd.SiteName, new CimscoPortal.Data.Models.Site { SiteName = "Holiday Inn" });


            foreach (string _entry in _sampleSites)
            {
                context.Sites.AddOrUpdate(
                    cd => cd.SiteName,
                        new CimscoPortal.Data.Models.Site
                        {
                            SiteName = _entry
                        }
                    );
            }
        }


        private void CreateGroupsAndCustomers(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            // Customers from CimscoNZ
            // ==> Script name : seed_Customers_table_from_source.sql
            context.Customers.AddOrUpdate(cd => cd.CustomerName, new CimscoPortal.Data.Models.Customer { CustomerName = "Customer not set" });
            context.Customers.AddOrUpdate(cd => cd.CustomerName, new CimscoPortal.Data.Models.Customer { CustomerName = "Field Nelson Holdings Ltd & Nelson Mega Ltd" });
            context.Customers.AddOrUpdate(cd => cd.CustomerName, new CimscoPortal.Data.Models.Customer { CustomerName = "Hewlett-Packard NZ" });
            context.Customers.AddOrUpdate(cd => cd.CustomerName, new CimscoPortal.Data.Models.Customer { CustomerName = "Intercontinental Group" });
            context.Customers.AddOrUpdate(cd => cd.CustomerName, new CimscoPortal.Data.Models.Customer { CustomerName = "Masterton Supermarkets Ltd" });
            context.Customers.AddOrUpdate(cd => cd.CustomerName, new CimscoPortal.Data.Models.Customer { CustomerName = "Nees Hardware Ltd" });
            //

            foreach (string _entry in _sampleCustomers)
            {
                context.Customers.AddOrUpdate(
                    cd => cd.CustomerName,
                        new CimscoPortal.Data.Models.Customer
                        {
                            CustomerName = _entry
                        }
                    );
            }
            //
            // ==> Script name : seed_Groups_table_from_source.sql
            context.Groups.AddOrUpdate(cd => cd.GroupName, new CimscoPortal.Data.Models.Group { GroupName = "Foodstuffs North Island" });
            context.Groups.AddOrUpdate(cd => cd.GroupName, new CimscoPortal.Data.Models.Group { GroupName = "Foodstuffs South Island" });
            context.Groups.AddOrUpdate(cd => cd.GroupName, new CimscoPortal.Data.Models.Group { GroupName = "Group not set" });
            context.Groups.AddOrUpdate(cd => cd.GroupName, new CimscoPortal.Data.Models.Group { GroupName = "Mitre 10 New Zealand" });
            //
            foreach (string _entry in _sampleGroups)
            {
                context.Groups.AddOrUpdate(
                    cd => cd.GroupName,
                        new CimscoPortal.Data.Models.Group
                        {
                            GroupName = _entry
                        }
                    );
            }
        }

        private void InvoiceDataSeed(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            // ==> script : seed_InvoiceSummaries_table_from_source.sql
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "January", Year = "2011", InvoiceId = 20, InvoiceDate = DateTime.Parse("02-01-2011"), InvoiceDueDate = DateTime.Parse("02-01-2011"), InvoiceNumber = "01022011", GstTotal = 3512.00M, InvoiceTotal = 26931.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4887.00M, EnergyChargesTotal = 14462.00M, MiscChargesTotal = 786.00M, TotalCharges = 23418.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "March", Year = "2010", InvoiceId = 21, InvoiceDate = DateTime.Parse("04-07-2010"), InvoiceDueDate = DateTime.Parse("04-07-2010"), InvoiceNumber = "336815", GstTotal = 2885.62M, InvoiceTotal = 25970.61M, AccountNumber = "8807926410", CustomerNumber = "400175020", SiteId = 2, NetworkChargesTotal = 4633.29M, EnergyChargesTotal = 17781.00M, MiscChargesTotal = 670.00M, TotalCharges = 23084.99M, GSTCharges = 2885.62M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "February", Year = "2011", InvoiceId = 22, InvoiceDate = DateTime.Parse("03-01-2011"), InvoiceDueDate = DateTime.Parse("03-01-2011"), InvoiceNumber = "01032011", GstTotal = 3020.00M, InvoiceTotal = 23155.00M, AccountNumber = "Unknown", CustomerNumber = "50419690", SiteId = 2, NetworkChargesTotal = 4887.00M, EnergyChargesTotal = 14462.00M, MiscChargesTotal = 749.00M, TotalCharges = 20135.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "January", Year = "2010", InvoiceId = 23, InvoiceDate = DateTime.Parse("02-01-2010"), InvoiceDueDate = DateTime.Parse("02-01-2010"), InvoiceNumber = "01022010", GstTotal = 2779.00M, InvoiceTotal = 25008.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4950.00M, EnergyChargesTotal = 16604.00M, MiscChargesTotal = 675.00M, TotalCharges = 22229.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "June", Year = "2011", InvoiceId = 24, InvoiceDate = DateTime.Parse("07-05-2011"), InvoiceDueDate = DateTime.Parse("07-05-2011"), InvoiceNumber = "656845", GstTotal = 2190.02M, InvoiceTotal = 16790.13M, AccountNumber = "2707108410", CustomerNumber = "Unknown", SiteId = 1, NetworkChargesTotal = 0.00M, EnergyChargesTotal = 0.00M, MiscChargesTotal = 0.00M, TotalCharges = 14600.11M, GSTCharges = 0.00M, ConnectionNumber = "ICP not set", SiteName = "Site not set", EnergyPointId = 1 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "February", Year = "2010", InvoiceId = 25, InvoiceDate = DateTime.Parse("03-01-2010"), InvoiceDueDate = DateTime.Parse("03-01-2010"), InvoiceNumber = "01032010", GstTotal = 2595.47M, InvoiceTotal = 23359.19M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4851.00M, EnergyChargesTotal = 15302.55M, MiscChargesTotal = 641.00M, TotalCharges = 20763.72M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "April", Year = "2010", InvoiceId = 26, InvoiceDate = DateTime.Parse("05-01-2010"), InvoiceDueDate = DateTime.Parse("05-01-2010"), InvoiceNumber = "01052010", GstTotal = 0.00M, InvoiceTotal = 0.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4601.00M, EnergyChargesTotal = 0.00M, MiscChargesTotal = 603.00M, TotalCharges = 0.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "May", Year = "2010", InvoiceId = 27, InvoiceDate = DateTime.Parse("06-01-2010"), InvoiceDueDate = DateTime.Parse("06-01-2010"), InvoiceNumber = "01062010", GstTotal = 3452.00M, InvoiceTotal = 31067.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4609.00M, EnergyChargesTotal = 22340.66M, MiscChargesTotal = 665.00M, TotalCharges = 27615.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "June", Year = "2010", InvoiceId = 28, InvoiceDate = DateTime.Parse("07-01-2010"), InvoiceDueDate = DateTime.Parse("07-01-2010"), InvoiceNumber = "01072010", GstTotal = 3429.00M, InvoiceTotal = 30862.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4655.00M, EnergyChargesTotal = 22135.97M, MiscChargesTotal = 642.00M, TotalCharges = 27433.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "July", Year = "2010", InvoiceId = 29, InvoiceDate = DateTime.Parse("08-01-2010"), InvoiceDueDate = DateTime.Parse("08-01-2010"), InvoiceNumber = "01082010", GstTotal = 3427.84M, InvoiceTotal = 30850.57M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4660.00M, EnergyChargesTotal = 22135.97M, MiscChargesTotal = 654.00M, TotalCharges = 27422.73M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "August", Year = "2010", InvoiceId = 30, InvoiceDate = DateTime.Parse("09-01-2010"), InvoiceDueDate = DateTime.Parse("09-01-2010"), InvoiceNumber = "01092010", GstTotal = 3202.00M, InvoiceTotal = 28815.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4635.00M, EnergyChargesTotal = 20320.40M, MiscChargesTotal = 658.00M, TotalCharges = 25613.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "September", Year = "2010", InvoiceId = 31, InvoiceDate = DateTime.Parse("10-01-2010"), InvoiceDueDate = DateTime.Parse("10-01-2010"), InvoiceNumber = "01102010", GstTotal = 2961.00M, InvoiceTotal = 26645.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4569.00M, EnergyChargesTotal = 18473.26M, MiscChargesTotal = 642.00M, TotalCharges = 23685.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "October", Year = "2010", InvoiceId = 32, InvoiceDate = DateTime.Parse("11-01-2010"), InvoiceDueDate = DateTime.Parse("11-01-2010"), InvoiceNumber = "01112010", GstTotal = 3338.46M, InvoiceTotal = 25594.83M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4660.00M, EnergyChargesTotal = 16918.93M, MiscChargesTotal = 660.00M, TotalCharges = 22256.37M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "November", Year = "2010", InvoiceId = 33, InvoiceDate = DateTime.Parse("12-01-2010"), InvoiceDueDate = DateTime.Parse("12-01-2010"), InvoiceNumber = "01122010", GstTotal = 3353.00M, InvoiceTotal = 25704.00M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 4940.00M, EnergyChargesTotal = 16738.13M, MiscChargesTotal = 673.00M, TotalCharges = 22351.00M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
            context.InvoiceSummaries.Add(new InvoiceSummary { Month = "December", Year = "2010", InvoiceId = 34, InvoiceDate = DateTime.Parse("01-01-2011"), InvoiceDueDate = DateTime.Parse("01-01-2011"), InvoiceNumber = "01012011", GstTotal = 3483.11M, InvoiceTotal = 26703.83M, AccountNumber = "Unknown", CustomerNumber = "Unknown", SiteId = 2, NetworkChargesTotal = 5000.00M, EnergyChargesTotal = 17503.04M, MiscChargesTotal = 700.00M, TotalCharges = 23220.72M, GSTCharges = 0.00M, ConnectionNumber = "0000103216TR397", SiteName = "Pak 'n Save Upper Hutt", EnergyPointId = 2 });
        }

        private void InvoiceDataSeed(CimscoPortal.Data.Models.CimscoPortalContext context, string siteName, int monthsToCreate)
        {
            DateTime _startDate = DateTime.Now.AddMonths(monthsToCreate * -1);
            DateTime _dueDate = new DateTime();
            //int _energyPoint = 200;
            Random rnd = new Random();
            System.Collections.Generic.List<InvoiceSummary> _returnData = new System.Collections.Generic.List<InvoiceSummary>();
            int _siteId = context.Sites.Where(x => x.SiteName == siteName).First().SiteId;
            string _siteName = context.Sites.Where(x => x.SiteName == siteName).First().SiteName;
            int _invoiceId = rnd.Next(100, 999) * _siteId;

            DateTime _endDate = DateTime.Now;
            AspNetUser _approver = new AspNetUser();
            decimal _lastTotal = 1;
            decimal _percentChange;
            while (_startDate < _endDate)
            {
                _dueDate = _startDate.AddMonths(1);
                int _invoiceNo = rnd.Next(1000000, 9999999);
                decimal _invoiceTotal = rnd.Next(8000, 12000);
                if (_invoiceTotal > 0)
                {
                    _percentChange = ((_lastTotal - _invoiceTotal) / _lastTotal) * 100;
                    if (Math.Abs(_percentChange) > 20.0M) { _percentChange = 0.0M; }
                }
                else { _percentChange = 0; }
                decimal _gstTotal = _invoiceTotal * 3.00M / 23.00M;
                decimal _networkChargesTotal = _invoiceTotal * 0.8M; decimal _energyChargesTotal = _invoiceTotal * 0.15M; decimal _miscChargesTotal = _invoiceTotal * 0.05M;
                DateTime _approvedDate = DateTime.Parse("01-01-2000");
                _approver = null;
                bool _approved = SetAllButLastTwoMonthsApproved(_startDate);
                if (_approved)
                {
                    _approver = context.Sites.Where(s => s.SiteId == _siteId).FirstOrDefault().Customer.Users.FirstOrDefault();
                    _approvedDate = DateTime.Today;
                }
                context.InvoiceSummaries.Add(new InvoiceSummary()
                {
                    Month = _startDate.ToString("MMMM"),
                    Year = _startDate.ToString("yyyy"),
                    InvoiceId = _invoiceId,
                    InvoiceDate = _startDate,
                    InvoiceDueDate = _dueDate,
                    InvoiceNumber = "t" + _invoiceNo.ToString(),
                    GstTotal = _gstTotal,
                    InvoiceTotal = _invoiceTotal,
                    AccountNumber = "Unknown",
                    CustomerNumber = "Unknown",
                    SiteId = _siteId,
                    NetworkChargesTotal = _networkChargesTotal,
                    EnergyChargesTotal = _energyChargesTotal,
                    MiscChargesTotal = _miscChargesTotal,
                    TotalCharges = 23418.00M,
                    GSTCharges = 0.00M,
                    TotalNetworkCharges = _networkChargesTotal,
                    TotalMiscCharges = _miscChargesTotal,
                    TotalEnergyCharges = _energyChargesTotal,
                    ConnectionNumber = "0000103216TR397",
                    SiteName = _siteName,
                    EnergyPointId = _siteId,
                    Approved = _approved,
                    UserId = _approver,
                    ApprovedDate = _approvedDate,
                    PercentageChange = _percentChange
                });
                _startDate = _startDate.AddMonths(1);
                _invoiceId++;
                //_energyPoint++;
                _lastTotal = _invoiceTotal;
            }
        }


        private void InvoiceDataSeedActual(CimscoPortal.Data.Models.CimscoPortalContext context)
        {
            //SiteName = "Pak 'n Save Upper Hutt"
            var _siteName = "Pak 'n Save Upper Hutt";
            int _invoiceId = 99991; // Will refer eventually back to core data
            int _siteId = context.Sites.Where(x => x.SiteName == _siteName).First().SiteId;

            //
            context.InvoiceSummaries.Add(new InvoiceSummary
            {
                Month = "",
                Year = "",
                InvoiceId = _invoiceId,
                InvoiceDate = DateTime.Parse("01-05-2015"),
                InvoiceDueDate = DateTime.Parse("01-05-2015"),
                InvoiceNumber = "614242671",
                GstTotal = 3725.59M,
                InvoiceTotal = 24837.26M,
                AccountNumber = "50684560",
                CustomerNumber = "11801549",
                SiteId = _siteId,
                NetworkChargesTotal = 5046.04M,
                EnergyChargesTotal = 19659.22M,
                MiscChargesTotal = 132.00M,
                TotalCharges = 0.0M,
                GSTCharges = 0.00M,
                ConnectionNumber = "0OO0103216TR397",
                SiteName = "Pak 'n Save Upper Hutt",
                EnergyPointId = 2,
                EnergyCharge = new EnergyCharge()
                {
                    BD0004 = 1039.83M,
                    BD0408 = 2117.64M,
                    BD0812 = 2975.10M,
                    BD1216 = 2757.23M,
                    BD1620 = 2711.67M,
                    BD2024 = 1700.31M,
                    NBD0004 = 597.57M,
                    NBD0408 = 857.08M,
                    NBD0812 = 1267.03M,
                    NBD1216 = 1136.19M,
                    NBD1620 = 1221.90M,
                    NBD2024 = 843.78M,

                    BD0004R = 7.8000M,
                    BD0408R = 9.1900M,
                    BD0812R = 11.3000M,
                    BD1216R = 10.8500M,
                    BD1620R = 12.0900M,
                    BD2024R = 9.7600M,
                    NBD0004R = 7.7400M,
                    NBD0408R = 7.1100M,
                    NBD0812R = 9.2600M,
                    NBD1216R = 8.5200M,
                    NBD1620R = 10.2800M,
                    NBD2024R = 8.8000M,

                    BDSVC = 149.37M,
                    BDSVCR = 0.1200M,
                    NBDSVC = 79.69M,
                    NBDSVCR = 0.1200M,

                    EALevy  = 204.83M,
                    EALevyR = 1.0438M, 

                    LossRate = 0.028M
                }
            });
        }


        private static bool SetAllButLastTwoMonthsApproved(DateTime _startDate)
        {
            bool _approved = !((_startDate.ToString("MMMM") == DateTime.Today.AddMonths(0).ToString("MMMM") || _startDate.ToString("MMMM") == DateTime.Today.AddMonths(-1).ToString("MMMM")) && _startDate.ToString("yyyy") == DateTime.Today.ToString("yyyy"));
            return _approved;
        }

        //private static string GetApprover(CimscoPortal.Data.Models.CimscoPortalContext context, int _siteId)
        //{
        //    string _approversId = context.Sites.Where(s => s.SiteId == _siteId).FirstOrDefault().Customer.Users.Select(x => x.Id).FirstOrDefault();

        //    if (_approversId == null || _approversId == "")
        //    {
        //        _approversId = "abc";
        //    }
        //    return _approversId;
        //}

        //var _invoiceTestData = new System.Collections.Generic.List<CompanyInvoiceViewModel>  {
        //                                            new CompanyInvoiceViewModel { CustomerId=1,  Month = DateTime.Parse("02-01-2011").ToString("MMM"), , YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Feb", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Mar", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Apr", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "May", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Jun", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "July", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Aug", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Sept", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Oct", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Nov", YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() },
        //                                            new CompanyInvoiceViewModel { CustomerId=1, Month = "Dec",YearA = rnd.Next(8000, 12000).ToString(), YearB = rnd.Next(8000, 12000).ToString() }
        //};

    }
        #endregion

}
// N'fa fa-phone bg-themeprimary white', N'fa fa-clock-o themeprimary', N'Phone Blue')