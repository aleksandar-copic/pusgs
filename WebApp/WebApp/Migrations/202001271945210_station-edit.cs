namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stationedit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "X", c => c.Double(nullable: false));
            AddColumn("dbo.Stations", "Y", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stations", "Y");
            DropColumn("dbo.Stations", "X");
        }
    }
}
