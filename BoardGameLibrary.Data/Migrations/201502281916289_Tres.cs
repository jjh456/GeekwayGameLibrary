namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tres : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Patrons", newName: "Attendees");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Attendees", newName: "Patrons");
        }
    }
}
