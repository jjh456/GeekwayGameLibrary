namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seis : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Copies", "OwnerName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Copies", "OwnerName", c => c.String(nullable: false));
        }
    }
}
