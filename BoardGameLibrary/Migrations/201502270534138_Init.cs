namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Copies", "OwnerName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Copies", "OwnerName", c => c.String());
            AlterColumn("dbo.Games", "Title", c => c.String());
        }
    }
}
