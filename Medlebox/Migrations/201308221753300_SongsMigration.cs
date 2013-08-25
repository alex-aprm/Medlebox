namespace Medlebox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        Name = c.String(maxLength: 500),
                        User_Gid = c.Guid(),
                    })
                .PrimaryKey(t => t.Gid)
                .ForeignKey("dbo.Users", t => t.User_Gid)
                .Index(t => t.User_Gid);
            
            CreateTable(
                "dbo.SongInPlaylists",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        NumOrder = c.Int(nullable: false),
                        Played = c.Boolean(nullable: false),
                        Song_Gid = c.Guid(),
                        Playlist_Gid = c.Guid(),
                    })
                .PrimaryKey(t => t.Gid)
                .ForeignKey("dbo.Songs", t => t.Song_Gid)
                .ForeignKey("dbo.Playlists", t => t.Playlist_Gid)
                .Index(t => t.Song_Gid)
                .Index(t => t.Playlist_Gid);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Gid = c.Guid(nullable: false),
                        Artist = c.String(maxLength: 500),
                        Title = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Gid);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SongInPlaylists", new[] { "Playlist_Gid" });
            DropIndex("dbo.SongInPlaylists", new[] { "Song_Gid" });
            DropIndex("dbo.Playlists", new[] { "User_Gid" });
            DropForeignKey("dbo.SongInPlaylists", "Playlist_Gid", "dbo.Playlists");
            DropForeignKey("dbo.SongInPlaylists", "Song_Gid", "dbo.Songs");
            DropForeignKey("dbo.Playlists", "User_Gid", "dbo.Users");
            DropTable("dbo.Songs");
            DropTable("dbo.SongInPlaylists");
            DropTable("dbo.Playlists");
        }
    }
}
