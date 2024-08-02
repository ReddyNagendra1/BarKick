namespace BarKick.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CocktailnBartender : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cocktails", "BartenderId", "dbo.Bartenders");
            DropIndex("dbo.Cocktails", new[] { "BartenderId" });
            DropTable("dbo.Cocktails");
            DropTable("dbo.Bartenders");
        }
    }
}
