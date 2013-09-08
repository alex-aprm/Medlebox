
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Reflection;
using RefactorThis.EFExtensions;
using AutoMapper;
using Medlebox.Models;
using System.ComponentModel;



namespace Medlebox.DAL
{
    public partial class MedleboxDAL
    {
        public User CurrentUser { get; set; }
        MedleboxDB db;
        public MedleboxDAL(string WeathertopConnection)
        {
            this.db = new MedleboxDB(WeathertopConnection);
            //God.GiveMeMaps();
        }
        public Song GetPlayable(Guid Gid)
        {
            Song song = this.GetSong(Gid);
            if (song == null)
            {
                Playlist playlist = this.GetPlaylist(Gid );
                if (playlist != null)
                    if (playlist.SongsCount > 0)
                        song = playlist.Songs.First().Song;
            }
            return song;
        }
        public void Dispose()
        {
            this.db.Dispose();
        }
    }

 }