namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ocho : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Value = c.Int(),
                        Game_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.ID)
                .ForeignKey("dbo.Games", t => t.Game_ID)
                .Index(t => t.ID)
                .Index(t => t.Game_ID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Attendee_ID = c.Int(),
                        Play_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Attendees", t => t.Attendee_ID)
                .ForeignKey("dbo.Plays", t => t.Play_ID)
                .Index(t => t.Attendee_ID)
                .Index(t => t.Play_ID);
            
            CreateTable(
                "dbo.Plays",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Checkouts", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Game_ID", "dbo.Games");
            DropForeignKey("dbo.Ratings", "ID", "dbo.Players");
            DropForeignKey("dbo.Players", "Play_ID", "dbo.Plays");
            DropForeignKey("dbo.Plays", "ID", "dbo.Checkouts");
            DropForeignKey("dbo.Players", "Attendee_ID", "dbo.Attendees");
            DropIndex("dbo.Plays", new[] { "ID" });
            DropIndex("dbo.Players", new[] { "Play_ID" });
            DropIndex("dbo.Players", new[] { "Attendee_ID" });
            DropIndex("dbo.Ratings", new[] { "Game_ID" });
            DropIndex("dbo.Ratings", new[] { "ID" });
            DropTable("dbo.Plays");
            DropTable("dbo.Players");
            DropTable("dbo.Ratings");
        }
    }
}
