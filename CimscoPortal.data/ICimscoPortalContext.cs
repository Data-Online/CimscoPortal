using System;
namespace CimscoPortal.Data
{
    public interface ICimscoPortalContext
    {
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.Address> Addresses { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.AspNetRole> AspNetRoles { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.AspNetUserClaim> AspNetUserClaims { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.AspNetUserLogin> AspNetUserLogins { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.AspNetUser> AspNetUsers { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.Contact> Contacts { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.Customer> Customers { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.Group> Groups { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.InvoiceSummary> InvoiceSummaries { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.MessageFormat> MessageFormats { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.MessageType> MessageTypes { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.PortalMessage> PortalMessages { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.SystemConfiguration> SystemConfiguration { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.City> Cities { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.Site> Sites { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.WelcomeScreen> WelcomeScreen { get; set; }
        System.Data.Entity.DbSet<CimscoPortal.Data.Models.UserSetting> UserSetting { get; set; }

        System.Data.Entity.DbSet<CimscoPortal.Data.Models.Division> Divisions { get; set; }

        System.Data.Entity.DbSet<CimscoPortal.Data.Models.sysdiagram> sysdiagrams { get; set; }

        void Commit();
        void Update(CimscoPortal.Data.Models.InvoiceSummary summary);
        void UpdateSettings(CimscoPortal.Data.Models.UserSetting settings);

    }
}
