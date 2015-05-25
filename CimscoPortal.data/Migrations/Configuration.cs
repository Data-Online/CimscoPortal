namespace CimscoPortal.Data.Migrations
{
    using CimscoPortal.Data.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CimscoPortal.Data.Models.CimscoPortalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
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

            context.Customers.AddOrUpdate(
            cd => cd.CustomerName,
                new Customer { CustomerName = "Customer not yet set" },
                new Customer { CustomerName = "Masterton Supermarkets Ltd" },
                new Customer { CustomerName = "Nees Hardware Ltd" },
                new Customer { CustomerName = "Hewlett-Packard NZ" },
                new Customer { CustomerName = "Field Nelson Holdings Ltd & Nelson Mega Ltd" },
                new Customer { CustomerName = "Intercontinental Group" }
                );

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
    }
}
// N'fa fa-phone bg-themeprimary white', N'fa fa-clock-o themeprimary', N'Phone Blue')