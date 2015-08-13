namespace Crowd.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedbackquery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParticipantResults", "FeedbackQuery", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParticipantResults", "FeedbackQuery");
        }
    }
}
