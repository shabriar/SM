namespace Stationary_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migForProductPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "UnitPriceUsd", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "UnitPriceUsd");
        }
    }
}
