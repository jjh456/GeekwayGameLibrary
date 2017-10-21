namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Siete : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Checkouts", "TimeIn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Checkouts", "TimeIn", c => c.DateTime(nullable: false));
        }
    }
}
