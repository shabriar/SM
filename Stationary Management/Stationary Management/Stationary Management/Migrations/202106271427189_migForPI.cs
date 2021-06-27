namespace Stationary_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migForPI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(),
                        Name = c.String(nullable: false),
                        HOAddress = c.String(),
                        FactoryAddress = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        IrcNo = c.String(),
                        TinNo = c.String(),
                        BinNo = c.String(),
                        ImportLicenseNo = c.String(),
                        ImportLicenseDateOfIssue = c.DateTime(),
                        ImportLicenseDateOfExpiry = c.DateTime(),
                        ImportRegistrationNo = c.String(),
                        BangladeshBankRegistrationNo = c.String(),
                        AcmId = c.Int(),
                        BankName = c.String(),
                        BankAcNumber = c.String(),
                        Branch = c.String(),
                        SwiftCode = c.String(),
                        IsSeller = c.Boolean(nullable: false),
                        IsBuyer = c.Boolean(nullable: false),
                        FaxNo = c.String(),
                        ParentGroupId = c.Int(),
                        Ext1 = c.String(),
                        Status = c.Byte(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AcmId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Customers", t => t.ParentGroupId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.AcmId)
                .Index(t => t.ParentGroupId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.ProformaInvoiceProductRelation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PfiId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        TotalPrice = c.Double(),
                        ShipmentMode = c.String(),
                        Remarks = c.String(),
                        Status = c.Byte(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.ProformaInvoices", t => t.PfiId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.PfiId)
                .Index(t => t.ProductId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.ProformaInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PfiNumber = c.String(),
                        PfiDate = c.DateTime(nullable: false),
                        BuyerId = c.Int(),
                        SellerId = c.Int(),
                        PfiValue = c.Double(),
                        TotalQuantity = c.Double(),
                        SeaFreightChargePerUnit = c.Double(),
                        AirFreightChargePerUnit = c.Double(),
                        ProductType = c.String(),
                        IncoTerms = c.String(),
                        PfiCategory = c.String(),
                        IndentValidity = c.String(),
                        ParentPfiId = c.Int(),
                        RevisedVersion = c.Int(),
                        DiscountType = c.String(),
                        DiscountValue = c.Double(),
                        PoNo = c.String(),
                        IsClosed = c.Boolean(),
                        RevisedText = c.String(),
                        Remarks = c.String(),
                        ForthComingLcDate = c.DateTime(),
                        CountryOfOrigins = c.String(),
                        Ext1 = c.String(),
                        Status = c.Byte(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.BuyerId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.ProformaInvoices", t => t.ParentPfiId)
                .ForeignKey("dbo.Customers", t => t.SellerId)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.BuyerId)
                .Index(t => t.SellerId)
                .Index(t => t.ParentPfiId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProformaInvoiceProductRelation", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ProformaInvoices", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.ProformaInvoices", "SellerId", "dbo.Customers");
            DropForeignKey("dbo.ProformaInvoiceProductRelation", "PfiId", "dbo.ProformaInvoices");
            DropForeignKey("dbo.ProformaInvoices", "ParentPfiId", "dbo.ProformaInvoices");
            DropForeignKey("dbo.ProformaInvoices", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.ProformaInvoices", "BuyerId", "dbo.Customers");
            DropForeignKey("dbo.ProformaInvoiceProductRelation", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProformaInvoiceProductRelation", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Customers", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Customers", "ParentGroupId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Customers", "AcmId", "dbo.Users");
            DropIndex("dbo.ProformaInvoices", new[] { "UpdatedBy" });
            DropIndex("dbo.ProformaInvoices", new[] { "CreatedBy" });
            DropIndex("dbo.ProformaInvoices", new[] { "ParentPfiId" });
            DropIndex("dbo.ProformaInvoices", new[] { "SellerId" });
            DropIndex("dbo.ProformaInvoices", new[] { "BuyerId" });
            DropIndex("dbo.ProformaInvoiceProductRelation", new[] { "UpdatedBy" });
            DropIndex("dbo.ProformaInvoiceProductRelation", new[] { "CreatedBy" });
            DropIndex("dbo.ProformaInvoiceProductRelation", new[] { "ProductId" });
            DropIndex("dbo.ProformaInvoiceProductRelation", new[] { "PfiId" });
            DropIndex("dbo.Customers", new[] { "UpdatedBy" });
            DropIndex("dbo.Customers", new[] { "CreatedBy" });
            DropIndex("dbo.Customers", new[] { "ParentGroupId" });
            DropIndex("dbo.Customers", new[] { "AcmId" });
            DropTable("dbo.ProformaInvoices");
            DropTable("dbo.ProformaInvoiceProductRelation");
            DropTable("dbo.Customers");
        }
    }
}
