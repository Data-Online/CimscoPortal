using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CimscoPortal.Data.Models.Mapping;

namespace CimscoPortal.Data.Models
{
    public partial class CimscoPortalContext : DbContext, CimscoPortal.Data.ICimscoPortalContext
    {

        static CimscoPortalContext()
        {
            //Database.SetInitializer<CimscoPortalContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CimscoPortalContext, CimscoPortal.Data.Migrations.Configuration>());
        }

        public CimscoPortalContext()
            : base("Name=CimscoPortalContext")
        //: base("Name=DefaultConnection")
        {
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public virtual void Update(InvoiceSummary _summary)
        {
            base.Entry(_summary).State = EntityState.Modified;
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
        public DbSet<City> Cities { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<EnergyCharge> EnergyCharges { get; set; }
        public DbSet<EnergyPoint> EnergyPoints { get; set; }
        public DbSet<LoginHistory> LoginHistorys { get; set; }
      //  public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new System.Data.Entity.ModelConfiguration.Conventions.DecimalPropertyConvention(12, 4));

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
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new SiteMap());
            modelBuilder.Configurations.Add(new EnergyChargeMap());
            modelBuilder.Configurations.Add(new EnergyPointMap());
            modelBuilder.Configurations.Add(new LoginHistoryMap());
          //  modelBuilder.Configurations.Add(new UserDetailMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
        }

    }
}
