using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Medlebox.Models;
using Medlebox.Migrations;
using System.Data.Entity.Migrations;
using System.Data.Common;

namespace Medlebox.DAL
{
    public class MedleboxDB: DbContext
    {
        public MedleboxDB(string ConnectionString)
            : base(ConnectionString)
        {
            God.MigrationConnectionString = ConnectionString;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MedleboxDB, Configuration>());
           // Configuration.ProxyCreationEnabled = false;
        }
        public MedleboxDB()
            : base(God.MigrationConnectionString ?? "Data Source=pc-alex;Initial Catalog=Medlebox;Integrated Security=true") //Только для миграций из Студии
        {
            //throw new Exception(this.Database.Connection.ConnectionString.ToString());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
       
    }

}