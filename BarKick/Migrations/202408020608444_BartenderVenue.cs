namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BartenderVenue : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.VenueTeams", newName: "TeamVenues");
            DropPrimaryKey("dbo.TeamVenues");
            CreateTable(
                "dbo.VenueBartenders",
                c => new
                    {
                        Venue_VenueID = c.Int(nullable: false),
                        Bartender_BartenderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Venue_VenueID, t.Bartender_BartenderId })
                .ForeignKey("dbo.Venues", t => t.Venue_VenueID, cascadeDelete: true)
                .ForeignKey("dbo.Bartenders", t => t.Bartender_BartenderId, cascadeDelete: true)
                .Index(t => t.Venue_VenueID)
                .Index(t => t.Bartender_BartenderId);
            
            AddPrimaryKey("dbo.TeamVenues", new[] { "Team_TeamID", "Venue_VenueID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VenueBartenders", "Bartender_BartenderId", "dbo.Bartenders");
            DropForeignKey("dbo.VenueBartenders", "Venue_VenueID", "dbo.Venues");
            DropIndex("dbo.VenueBartenders", new[] { "Bartender_BartenderId" });
            DropIndex("dbo.VenueBartenders", new[] { "Venue_VenueID" });
            DropPrimaryKey("dbo.TeamVenues");
            DropTable("dbo.VenueBartenders");
            AddPrimaryKey("dbo.TeamVenues", new[] { "Venue_VenueID", "Team_TeamID" });
            RenameTable(name: "dbo.TeamVenues", newName: "VenueTeams");
        }
    }
}
