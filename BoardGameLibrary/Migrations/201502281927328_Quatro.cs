namespace BoardGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Quatro : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Checkouts", name: "Patron_ID", newName: "Attendee_ID");
            RenameIndex(table: "dbo.Checkouts", name: "IX_Patron_ID", newName: "IX_Attendee_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Checkouts", name: "IX_Attendee_ID", newName: "IX_Patron_ID");
            RenameColumn(table: "dbo.Checkouts", name: "Attendee_ID", newName: "Patron_ID");
        }
    }
}
