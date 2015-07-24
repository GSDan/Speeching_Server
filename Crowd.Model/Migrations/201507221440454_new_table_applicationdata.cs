namespace Crowd.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_table_applicationdata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationDatas",
                c => new
                    {
                        ApplicationName = c.String(nullable: false, maxLength: 128),
                        LoweredApplicationName = c.String(),
                        ApplicationId = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplicationDatas");
        }
    }
}
