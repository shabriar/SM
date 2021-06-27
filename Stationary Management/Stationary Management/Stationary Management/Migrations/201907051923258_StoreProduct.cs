namespace Stationary_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(),
                        ProductName = c.String(nullable: false),
                        Details = c.String(),
                        StockAmount = c.Double(nullable: false),
                        Status = c.Byte(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductsId = c.Int(),
                        Quantity = c.Double(nullable: false),
                        Status = c.Byte(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductsId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.ProductsId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Stores", "ProductsId", "dbo.Products");
            DropForeignKey("dbo.Stores", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Products", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Stores", new[] { "UpdatedBy" });
            DropIndex("dbo.Stores", new[] { "CreatedBy" });
            DropIndex("dbo.Stores", new[] { "ProductsId" });
            DropIndex("dbo.Products", new[] { "UpdatedBy" });
            DropIndex("dbo.Products", new[] { "CreatedBy" });
            DropTable("dbo.Stores");
            DropTable("dbo.Products");
        }
    }
}
