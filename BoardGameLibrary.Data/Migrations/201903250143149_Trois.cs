namespace BoardGameLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trois : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameCollections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Games", "GameCollectionID", c => c.Int());
            CreateIndex("dbo.Games", "GameCollectionID");
            AddForeignKey("dbo.Games", "GameCollectionID", "dbo.GameCollections", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "GameCollectionID", "dbo.GameCollections");
            DropIndex("dbo.Games", new[] { "GameCollectionID" });
            DropColumn("dbo.Games", "GameCollectionID");
            DropTable("dbo.GameCollections");
        }
    }
}
