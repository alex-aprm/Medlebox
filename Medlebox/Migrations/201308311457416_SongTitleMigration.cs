namespace Medlebox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongTitleMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "TitleWithoutArticle", c => c.String());
            Sql("Update Songs set TitleWithoutArticle=ltrim(rtrim(replace(Title,'the','')))");
            Sql(@"CREATE TRIGGER TitleWithoutArticle ON Songs AFTER UPDATE,INSERT AS 
                  BEGIN
                  	Update Songs set TitleWithoutArticle=ltrim(rtrim(replace(Title,'the','')))
                  END");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER TitleWithoutArticle ON Songs");
            DropColumn("dbo.Songs", "TitleWithoutArticle");
        }
    }
}
