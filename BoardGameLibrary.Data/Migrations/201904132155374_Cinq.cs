namespace BoardGameLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cinq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Copies", "Winnable", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.CopyCollections", "Color", c => c.String());
            AddColumn("dbo.CopyCollections", "AllowWinning", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CopyCollections", "AllowWinning");
            DropColumn("dbo.CopyCollections", "Color");
            DropColumn("dbo.Copies", "Winnable");
        }
    }
}
