namespace BoardGameLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Un : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BadgeID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Checkouts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeOut = c.DateTime(nullable: false),
                        TimeIn = c.DateTime(),
                        Attendee_ID = c.Int(),
                        Copy_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Attendees", t => t.Attendee_ID)
                .ForeignKey("dbo.Copies", t => t.Copy_ID)
                .Index(t => t.Attendee_ID)
                .Index(t => t.Copy_ID);
            
            CreateTable(
                "dbo.Copies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LibraryID = c.Int(nullable: false),
                        GameID = c.Int(nullable: false),
                        OwnerName = c.String(),
                        Notes = c.String(),
                        CurrentCheckout_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Checkouts", t => t.CurrentCheckout_ID)
                .ForeignKey("dbo.Games", t => t.GameID, cascadeDelete: true)
                .Index(t => t.GameID)
                .Index(t => t.CurrentCheckout_ID);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Value = c.Int(),
                        Game_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.ID)
                .ForeignKey("dbo.Games", t => t.Game_ID)
                .Index(t => t.ID)
                .Index(t => t.Game_ID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Attendee_ID = c.Int(),
                        Play_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Attendees", t => t.Attendee_ID)
                .ForeignKey("dbo.Plays", t => t.Play_ID)
                .Index(t => t.Attendee_ID)
                .Index(t => t.Play_ID);
            
            CreateTable(
                "dbo.Plays",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Checkouts", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Checkouts", "Copy_ID", "dbo.Copies");
            DropForeignKey("dbo.Ratings", "Game_ID", "dbo.Games");
            DropForeignKey("dbo.Ratings", "ID", "dbo.Players");
            DropForeignKey("dbo.Players", "Play_ID", "dbo.Plays");
            DropForeignKey("dbo.Plays", "ID", "dbo.Checkouts");
            DropForeignKey("dbo.Players", "Attendee_ID", "dbo.Attendees");
            DropForeignKey("dbo.Copies", "GameID", "dbo.Games");
            DropForeignKey("dbo.Copies", "CurrentCheckout_ID", "dbo.Checkouts");
            DropForeignKey("dbo.Checkouts", "Attendee_ID", "dbo.Attendees");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Plays", new[] { "ID" });
            DropIndex("dbo.Players", new[] { "Play_ID" });
            DropIndex("dbo.Players", new[] { "Attendee_ID" });
            DropIndex("dbo.Ratings", new[] { "Game_ID" });
            DropIndex("dbo.Ratings", new[] { "ID" });
            DropIndex("dbo.Copies", new[] { "CurrentCheckout_ID" });
            DropIndex("dbo.Copies", new[] { "GameID" });
            DropIndex("dbo.Checkouts", new[] { "Copy_ID" });
            DropIndex("dbo.Checkouts", new[] { "Attendee_ID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Plays");
            DropTable("dbo.Players");
            DropTable("dbo.Ratings");
            DropTable("dbo.Games");
            DropTable("dbo.Copies");
            DropTable("dbo.Checkouts");
            DropTable("dbo.Attendees");
        }
    }
}
