namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FOURTH : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "TypeId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
            AddColumn("dbo.AspNetUsers", "VerificateAcc", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ConfirmPassword", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "ImageUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "Date", c => c.String());
            CreateIndex("dbo.AspNetUsers", "TypeId");
            AddForeignKey("dbo.AspNetUsers", "TypeId", "dbo.UserTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "TypeId", "dbo.UserTypes");
            DropIndex("dbo.AspNetUsers", new[] { "TypeId" });
            DropColumn("dbo.AspNetUsers", "Date");
            DropColumn("dbo.AspNetUsers", "ImageUrl");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "ConfirmPassword");
            DropColumn("dbo.AspNetUsers", "VerificateAcc");
            DropColumn("dbo.AspNetUsers", "Password");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "TypeId");
            DropTable("dbo.UserTypes");
        }
    }
}
