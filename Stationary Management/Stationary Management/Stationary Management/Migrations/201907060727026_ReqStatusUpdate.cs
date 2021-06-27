namespace Stationary_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReqStatusUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requisitions", "ReqStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Requisitions", "Status", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requisitions", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.Requisitions", "ReqStatus");
        }
    }
}
