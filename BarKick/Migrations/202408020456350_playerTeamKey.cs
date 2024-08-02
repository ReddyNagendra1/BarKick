namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playerTeamKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Players", "Team_TeamID", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_TeamID" });
            RenameColumn(table: "dbo.Players", name: "Team_TeamID", newName: "TeamID");
            AddColumn("dbo.Players", "TeamName", c => c.String());
            AlterColumn("dbo.Players", "TeamID", c => c.Int(nullable: false));
            CreateIndex("dbo.Players", "TeamID");
            AddForeignKey("dbo.Players", "TeamID", "dbo.Teams", "TeamID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "TeamID", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "TeamID" });
            AlterColumn("dbo.Players", "TeamID", c => c.Int());
            DropColumn("dbo.Players", "TeamName");
            RenameColumn(table: "dbo.Players", name: "TeamID", newName: "Team_TeamID");
            CreateIndex("dbo.Players", "Team_TeamID");
            AddForeignKey("dbo.Players", "Team_TeamID", "dbo.Teams", "TeamID");
        }
    }
}
