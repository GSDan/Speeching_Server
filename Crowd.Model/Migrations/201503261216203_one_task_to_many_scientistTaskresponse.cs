namespace Crowd.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one_task_to_many_scientistTaskresponse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScientistTaskResponses", "ParticipantTaskId", "dbo.ParticipantTasks");
            DropPrimaryKey("dbo.ScientistTaskResponses");
            AddColumn("dbo.ScientistTaskResponses", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ScientistTaskResponses", "Id");
            AddForeignKey("dbo.ScientistTaskResponses", "ParticipantTaskId", "dbo.ParticipantTasks", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScientistTaskResponses", "ParticipantTaskId", "dbo.ParticipantTasks");
            DropPrimaryKey("dbo.ScientistTaskResponses");
            DropColumn("dbo.ScientistTaskResponses", "Id");
            AddPrimaryKey("dbo.ScientistTaskResponses", "ParticipantTaskId");
            AddForeignKey("dbo.ScientistTaskResponses", "ParticipantTaskId", "dbo.ParticipantTasks", "Id");
        }
    }
}
