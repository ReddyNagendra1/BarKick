namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamtoVenue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "Venue_VenueID", "dbo.Venues");
            DropIndex("dbo.Teams", new[] { "Venue_VenueID" });
            CreateTable(
                "dbo.VenueTeams",
                c => new
                    {
                        Venue_VenueID = c.Int(nullable: false),
                        Team_TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Venue_VenueID, t.Team_TeamID })
                .ForeignKey("dbo.Venues", t => t.Venue_VenueID, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_TeamID, cascadeDelete: true)
                .Index(t => t.Venue_VenueID)
                .Index(t => t.Team_TeamID);
            
            DropColumn("dbo.Teams", "Venue_VenueID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teams", "Venue_VenueID", c => c.Int());
            DropForeignKey("dbo.VenueTeams", "Team_TeamID", "dbo.Teams");
            DropForeignKey("dbo.VenueTeams", "Venue_VenueID", "dbo.Venues");
            DropIndex("dbo.VenueTeams", new[] { "Team_TeamID" });
            DropIndex("dbo.VenueTeams", new[] { "Venue_VenueID" });
            DropTable("dbo.VenueTeams");
            CreateIndex("dbo.Teams", "Venue_VenueID");
            AddForeignKey("dbo.Teams", "Venue_VenueID", "dbo.Venues", "VenueID");
        }
    }
}
