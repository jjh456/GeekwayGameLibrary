namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Uno : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Copies", "Game_ID", "dbo.Games");
            DropIndex("dbo.Copies", new[] { "Game_ID" });
            RenameColumn(table: "dbo.Copies", name: "Game_ID", newName: "GameID");
            AlterColumn("dbo.Copies", "GameID", c => c.Int(nullable: false));
            CreateIndex("dbo.Copies", "GameID");
            AddForeignKey("dbo.Copies", "GameID", "dbo.Games", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Copies", "GameID", "dbo.Games");
            DropIndex("dbo.Copies", new[] { "GameID" });
            AlterColumn("dbo.Copies", "GameID", c => c.Int());
            RenameColumn(table: "dbo.Copies", name: "GameID", newName: "Game_ID");
            CreateIndex("dbo.Copies", "Game_ID");
            AddForeignKey("dbo.Copies", "Game_ID", "dbo.Games", "ID");
        }
    }
}
