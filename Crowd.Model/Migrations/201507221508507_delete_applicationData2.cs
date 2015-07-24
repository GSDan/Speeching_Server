namespace Crowd.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_applicationData2 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ApplicationDatas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationDatas",
                c => new
                    {
                        ApplicationId = c.Guid(nullable: false),
                        ApplicationName = c.String(),
                        LoweredApplicationName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationId);
            
        }
    }
}
