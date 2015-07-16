namespace CimscoPortal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "GroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Customer_CustomerId", c => c.Int());
            CreateIndex("dbo.Groups", "Customer_CustomerId");
            AddForeignKey("dbo.Groups", "Customer_CustomerId", "dbo.Customers", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Customer_CustomerId", "dbo.Customers");
            DropIndex("dbo.Groups", new[] { "Customer_CustomerId" });
            DropColumn("dbo.Groups", "Customer_CustomerId");
            DropColumn("dbo.Customers", "GroupId");
        }
    }
}
