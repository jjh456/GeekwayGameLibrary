namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cinco : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false));
        }
    }
}
