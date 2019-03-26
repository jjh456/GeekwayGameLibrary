namespace BoardGameLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Troix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CopyCollections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Copies", "Title", c => c.String());
            AddColumn("dbo.Copies", "CopyCollectionID", c => c.Int());
            CreateIndex("dbo.Copies", "CopyCollectionID");
            AddForeignKey("dbo.Copies", "CopyCollectionID", "dbo.CopyCollections", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Copies", "CopyCollectionID", "dbo.CopyCollections");
            DropIndex("dbo.Copies", new[] { "CopyCollectionID" });
            DropColumn("dbo.Copies", "CopyCollectionID");
            DropColumn("dbo.Copies", "Title");
            DropTable("dbo.CopyCollections");
        }
    }
}
