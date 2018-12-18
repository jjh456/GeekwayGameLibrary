namespace BoardGameLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deux : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "WantsToWin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "WantsToWin");
        }
    }
}
