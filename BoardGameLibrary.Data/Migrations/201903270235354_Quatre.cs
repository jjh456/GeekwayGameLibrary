namespace BoardGameLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Quatre : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Copies", "Title");
            AlterColumn("dbo.Copies", "LibraryID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Copies", "LibraryID", c => c.Int(nullable: false));
            AddColumn("dbo.Copies", "Title", c => c.String());
        }
    }
}
