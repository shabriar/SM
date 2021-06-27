namespace Stationary_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Guid(nullable: false),
                        EventType = c.String(),
                        TableName = c.String(nullable: false),
                        PrimaryKeyName = c.String(),
                        PrimaryKeyValue = c.String(),
                        ColumnName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        CreatedUser = c.Int(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AuditLogId)
                .ForeignKey("dbo.Users", t => t.CreatedUser)
                .Index(t => t.CreatedUser);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 1000),
                        ShortName = c.String(),
                        UserName = c.String(nullable: false, maxLength: 20),
                        FatherName = c.String(),
                        MotherName = c.String(),
                        SpouseName = c.String(),
                        Email = c.String(maxLength: 1000),
                        Password = c.String(nullable: false, maxLength: 1000),
                        MobileNumber = c.String(maxLength: 1000),
                        EmergencyContNumber = c.String(maxLength: 1000),
                        PhoneExt = c.String(maxLength: 1000),
                        PresentAddress = c.String(maxLength: 1000),
                        PermanentAddress = c.String(maxLength: 1000),
                        Gender = c.String(maxLength: 1000),
                        SupUser = c.Boolean(nullable: false),
                        ImageFile = c.String(maxLength: 1000),
                        RoleId = c.Int(),
                        JoiningDate = c.DateTime(),
                        BirthDate = c.DateTime(),
                        LastPassword = c.String(maxLength: 1000),
                        LastPassChangeDate = c.DateTime(),
                        PasswordChangedCount = c.Int(),
                        SecurityStamp = c.String(maxLength: 1000),
                        CardNo = c.String(maxLength: 1000),
                        UserType = c.String(maxLength: 1000),
                        EmployeeType = c.String(maxLength: 1000),
                        ExpireDate = c.DateTime(),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(),
                        Status = c.Byte(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.Int(),
                        UpdatedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        UserRole_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRoles", t => t.UserRole_Id)
                .ForeignKey("dbo.UserRoles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.UserRole_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 1000),
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
                "dbo.UserRolePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Permission = c.String(maxLength: 1000),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditLogs", "CreatedUser", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.UserRoles");
            DropForeignKey("dbo.Users", "UserRole_Id", "dbo.UserRoles");
            DropForeignKey("dbo.UserRoles", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.UserRolePermissions", "RoleId", "dbo.UserRoles");
            DropForeignKey("dbo.UserRoles", "CreatedBy", "dbo.Users");
            DropIndex("dbo.UserRolePermissions", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UpdatedBy" });
            DropIndex("dbo.UserRoles", new[] { "CreatedBy" });
            DropIndex("dbo.Users", new[] { "UserRole_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.AuditLogs", new[] { "CreatedUser" });
            DropTable("dbo.UserRolePermissions");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.AuditLogs");
        }
    }
}
