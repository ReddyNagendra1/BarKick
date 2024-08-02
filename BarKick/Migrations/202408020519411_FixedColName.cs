namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedColName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Players", "TeamName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "TeamName", c => c.String());
        }
    }
}
