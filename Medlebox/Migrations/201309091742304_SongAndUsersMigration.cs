namespace Medlebox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongAndUsersMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "NowPlaying_Gid", c => c.Guid());
            AddForeignKey("dbo.Users", "NowPlaying_Gid", "dbo.SongInPlaylists", "Gid");
            CreateIndex("dbo.Users", "NowPlaying_Gid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "NowPlaying_Gid" });
            DropForeignKey("dbo.Users", "NowPlaying_Gid", "dbo.SongInPlaylists");
            DropColumn("dbo.Users", "NowPlaying_Gid");
        }
    }
}
