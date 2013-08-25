namespace Medlebox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongsMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Songs", "Artist", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Songs", "Title", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Songs", "Title", c => c.String(maxLength: 500));
            AlterColumn("dbo.Songs", "Artist", c => c.String(maxLength: 500));
        }
    }
}
