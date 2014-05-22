namespace wwwroot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyPuzzles",
                c => new
                    {
                        DailyPuzzleId = c.Int(nullable: false, identity: true),
                        GameGroupId = c.Int(nullable: false),
                        DateWhenActive = c.DateTime(),
                        DateCreated = c.DateTime(nullable: false),
                        IPAddress = c.String(),
                    })
                .PrimaryKey(t => t.DailyPuzzleId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Message = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IPAddress = c.String(),
                    })
                .PrimaryKey(t => t.FeedbackId);
            
            CreateTable(
                "dbo.GameGroups",
                c => new
                    {
                        GameGroupId = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        Level = c.String(),
                        IsDeleted = c.Boolean(),
                        Language = c.String(),
                        GameGroupId2 = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GameGroupId)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GameId);
            
            CreateTable(
                "dbo.GameWords",
                c => new
                    {
                        GameWordId = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        WordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameWordId)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.WordId);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        WordId = c.Int(nullable: false, identity: true),
                        Word1 = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WordId);
            
            CreateTable(
                "dbo.GameVariants",
                c => new
                    {
                        GameVariantId = c.Int(nullable: false, identity: true),
                        X1 = c.Short(nullable: false),
                        Y1 = c.Short(nullable: false),
                        X2 = c.Short(nullable: false),
                        Y2 = c.Short(nullable: false),
                        WordId = c.Int(nullable: false),
                        GameGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameVariantId)
                .ForeignKey("dbo.GameGroups", t => t.GameGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .Index(t => t.GameGroupId)
                .Index(t => t.WordId);
            
            CreateTable(
                "dbo.GameHistories",
                c => new
                    {
                        GameHistoryId = c.Int(nullable: false, identity: true),
                        GameGroupId = c.Int(nullable: false),
                        DateStarted = c.DateTime(),
                        DateFinished = c.DateTime(),
                        IpAddress = c.String(),
                        PlayerName = c.String(),
                        Score = c.Int(),
                    })
                .PrimaryKey(t => t.GameHistoryId);
            
            CreateTable(
                "dbo.Widgets",
                c => new
                    {
                        WidgetId = c.Int(nullable: false, identity: true),
                        GameGroupId = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedBy = c.String(),
                        IPAddress = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WidgetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameWords", "WordId", "dbo.Words");
            DropForeignKey("dbo.GameVariants", "WordId", "dbo.Words");
            DropForeignKey("dbo.GameVariants", "GameGroupId", "dbo.GameGroups");
            DropForeignKey("dbo.GameWords", "GameId", "dbo.Games");
            DropForeignKey("dbo.GameGroups", "GameId", "dbo.Games");
            DropIndex("dbo.GameWords", new[] { "WordId" });
            DropIndex("dbo.GameVariants", new[] { "WordId" });
            DropIndex("dbo.GameVariants", new[] { "GameGroupId" });
            DropIndex("dbo.GameWords", new[] { "GameId" });
            DropIndex("dbo.GameGroups", new[] { "GameId" });
            DropTable("dbo.Widgets");
            DropTable("dbo.GameHistories");
            DropTable("dbo.GameVariants");
            DropTable("dbo.Words");
            DropTable("dbo.GameWords");
            DropTable("dbo.Games");
            DropTable("dbo.GameGroups");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.DailyPuzzles");
        }
    }
}
