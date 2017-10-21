namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Copies", "LibraryID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Copies", "LibraryID");
        }
    }
}
