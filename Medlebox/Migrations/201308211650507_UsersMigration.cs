namespace Medlebox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        Email = c.String(maxLength: 200),
                        Nickname = c.String(maxLength: 200),
                        PwdHash = c.Binary(maxLength: 500),
                    })
                .PrimaryKey(t => t.Gid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
