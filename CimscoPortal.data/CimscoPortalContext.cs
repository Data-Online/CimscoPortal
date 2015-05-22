using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CimscoPortal.Data.Models.Mapping;

namespace CimscoPortal.Data.Models
{
    public partial class CimscoPortalContext : DbContext, CimscoPortal.Data.ICimscoPortalContext
    {
        static CimscoPortalContext()
        {
            Database.SetInitializer<CimscoPortalContext>(null);
        }

        public CimscoPortalContext()
            //: base("Name=CimscoPortalContext")
            : base("Name=DefaultConnection")
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<InvoiceSummary> InvoiceSummaries { get; set; }
        public DbSet<MessageFormat> MessageFormats { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<PortalMessage> PortalMessages { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new InvoiceSummaryMap());
            modelBuilder.Configurations.Add(new MessageFormatMap());
            modelBuilder.Configurations.Add(new MessageTypeMap());
            modelBuilder.Configurations.Add(new PortalMessageMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
        }
    }
}
