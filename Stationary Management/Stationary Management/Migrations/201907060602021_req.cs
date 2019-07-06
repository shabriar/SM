namespace Stationary_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class req : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductsId = c.Int(),
                        RequisitionQuantity = c.Double(nullable: false),
                        RequisitionNo = c.Double(nullable: false),
                        Status = c.Boolean(nullable: false),
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
            DropForeignKey("dbo.Requisitions", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Requisitions", "ProductsId", "dbo.Products");
            DropForeignKey("dbo.Requisitions", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Requisitions", new[] { "UpdatedBy" });
            DropIndex("dbo.Requisitions", new[] { "CreatedBy" });
            DropIndex("dbo.Requisitions", new[] { "ProductsId" });
            DropTable("dbo.Requisitions");
        }
    }
}
