namespace Crowd.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultiApps2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParticipantActivityCategories", "App", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "App", c => c.Int(nullable: false));
            AddColumn("dbo.ParticipantFeedItems", "App", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParticipantFeedItems", "App");
            DropColumn("dbo.Users", "App");
            DropColumn("dbo.ParticipantActivityCategories", "App");
        }
    }
}
