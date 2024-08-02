namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Venues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        VenueID = c.Int(nullable: false, identity: true),
                        VenueName = c.String(),
                        VenueLocation = c.String(),
                    })
                .PrimaryKey(t => t.VenueID);
            
            AddColumn("dbo.Teams", "Venue_VenueID", c => c.Int());
            CreateIndex("dbo.Teams", "Venue_VenueID");
            AddForeignKey("dbo.Teams", "Venue_VenueID", "dbo.Venues", "VenueID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "Venue_VenueID", "dbo.Venues");
            DropIndex("dbo.Teams", new[] { "Venue_VenueID" });
            DropColumn("dbo.Teams", "Venue_VenueID");
            DropTable("dbo.Venues");
        }
    }
}
