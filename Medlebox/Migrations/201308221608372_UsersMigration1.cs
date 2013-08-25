namespace Medlebox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersMigration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MusicSource", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "MusicSource");
        }
    }
}
