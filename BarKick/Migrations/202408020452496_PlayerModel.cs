namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                        PlayerPosition = c.String(),
                        Team_TeamID = c.Int(),
                    })
                .PrimaryKey(t => t.PlayerID)
                .ForeignKey("dbo.Teams", t => t.Team_TeamID)
                .Index(t => t.Team_TeamID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Team_TeamID", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_TeamID" });
            DropTable("dbo.Players");
        }
    }
}
