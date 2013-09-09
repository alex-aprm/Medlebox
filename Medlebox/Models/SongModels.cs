using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Medlebox.Models
{
    public class Song
    {
        public Song()
        {
            this.Gid = Guid.NewGuid();
        }
        [Key]
        public Guid Gid { get; set; }
        [MaxLength(500)]
        [Required(ErrorMessage="Кто исполняет?")]
        public string Artist { get; set; }
        [MaxLength(500)]
        [Required(ErrorMessage = "Как называется песня?")]
        public string Title { get; set; }
        public string TitleWithoutArticle { get; set; }
        public string Name
        {
            get
            {
                return this.Artist + " - " + this.Title;
            }
        }
    }

    public class SongInPlaylist
    {
        public SongInPlaylist()
        {
            this.Gid = Guid.NewGuid();
        }
        [Key]
        public Guid Gid { get; set; }
        public virtual Song Song { get; set; }
        public virtual int NumOrder { get; set; }
        public bool Played { get; set; }
        public virtual Playlist Playlist { get; set; }
    }

    public class Playlist
    {
        public Playlist()
        {
            this.Gid = Guid.NewGuid();
            this.Songs = new List<SongInPlaylist>();
        }

        [Key]
        public Guid Gid { get; set; }
        public virtual User User { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public virtual List<SongInPlaylist> Songs { get; set; }
        public int SongsCount { get { return this.Songs.Count(); } }
    }
}