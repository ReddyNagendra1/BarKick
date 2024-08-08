namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bartenders",
                c => new
                    {
                        BartenderId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.BartenderId);
            
            CreateTable(
                "dbo.VenueBartenders",
                c => new
                    {
                        BartenderId = c.Int(nullable: false),
                        VenueID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BartenderId, t.VenueID })
                .ForeignKey("dbo.Bartenders", t => t.BartenderId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.VenueID, cascadeDelete: true)
                .Index(t => t.BartenderId)
                .Index(t => t.VenueID);
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        VenueID = c.Int(nullable: false, identity: true),
                        VenueName = c.String(),
                        VenueLocation = c.String(),
                    })
                .PrimaryKey(t => t.VenueID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                        TeamBio = c.String(),
                    })
                .PrimaryKey(t => t.TeamID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                        PlayerPosition = c.String(),
                        TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerID)
                .ForeignKey("dbo.Teams", t => t.TeamID, cascadeDelete: true)
                .Index(t => t.TeamID);
            
            CreateTable(
                "dbo.Cocktails",
                c => new
                    {
                        DrinkId = c.Int(nullable: false, identity: true),
                        DrinkName = c.String(),
                        DrinkRecipe = c.String(),
                        LiqIn = c.String(),
                        MixIn = c.String(),
                        BartenderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DrinkId)
                .ForeignKey("dbo.Bartenders", t => t.BartenderId, cascadeDelete: true)
                .Index(t => t.BartenderId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TeamVenues",
                c => new
                    {
                        Team_TeamID = c.Int(nullable: false),
                        Venue_VenueID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_TeamID, t.Venue_VenueID })
                .ForeignKey("dbo.Teams", t => t.Team_TeamID, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.Venue_VenueID, cascadeDelete: true)
                .Index(t => t.Team_TeamID)
                .Index(t => t.Venue_VenueID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Cocktails", "BartenderId", "dbo.Bartenders");
            DropForeignKey("dbo.VenueBartenders", "VenueID", "dbo.Venues");
            DropForeignKey("dbo.TeamVenues", "Venue_VenueID", "dbo.Venues");
            DropForeignKey("dbo.TeamVenues", "Team_TeamID", "dbo.Teams");
            DropForeignKey("dbo.Players", "TeamID", "dbo.Teams");
            DropForeignKey("dbo.VenueBartenders", "BartenderId", "dbo.Bartenders");
            DropIndex("dbo.TeamVenues", new[] { "Venue_VenueID" });
            DropIndex("dbo.TeamVenues", new[] { "Team_TeamID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Cocktails", new[] { "BartenderId" });
            DropIndex("dbo.Players", new[] { "TeamID" });
            DropIndex("dbo.VenueBartenders", new[] { "VenueID" });
            DropIndex("dbo.VenueBartenders", new[] { "BartenderId" });
            DropTable("dbo.TeamVenues");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Cocktails");
            DropTable("dbo.Players");
            DropTable("dbo.Teams");
            DropTable("dbo.Venues");
            DropTable("dbo.VenueBartenders");
            DropTable("dbo.Bartenders");
        }
    }
}
