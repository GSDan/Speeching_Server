namespace Crowd.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultiApps5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParticipantActivities", "AppType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParticipantActivities", "AppType");
        }
    }
}
