namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinalPrice = c.Double(nullable: false),
                        PricelistId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceLists", t => t.PricelistId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PricelistId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TicketPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        PricelistId = c.Int(nullable: false),
                        TicketTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceLists", t => t.PricelistId, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TicketTypeId, cascadeDelete: true)
                .Index(t => t.PricelistId)
                .Index(t => t.TicketTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketPrices", "TicketTypeId", "dbo.TicketTypes");
            DropForeignKey("dbo.TicketPrices", "PricelistId", "dbo.PriceLists");
            DropForeignKey("dbo.Tickets", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "PricelistId", "dbo.PriceLists");
            DropIndex("dbo.TicketPrices", new[] { "TicketTypeId" });
            DropIndex("dbo.TicketPrices", new[] { "PricelistId" });
            DropIndex("dbo.Tickets", new[] { "UserId" });
            DropIndex("dbo.Tickets", new[] { "PricelistId" });
            DropTable("dbo.TicketPrices");
            DropTable("dbo.Tickets");
            DropTable("dbo.PriceLists");
        }
    }
}
