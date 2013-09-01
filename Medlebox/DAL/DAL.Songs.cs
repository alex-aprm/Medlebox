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


        public List<Song> GetSongs()
        {
            List<Song> l = db.Songs.AsNoTracking().ToList();
            return l;
        }

        public List<Song> GetSongsBySearch(string q)
        {
            q = q.ToLower().Replace("the", "").Trim() ;
            string[] ql = q.Split(new string[] {" - "},StringSplitOptions.RemoveEmptyEntries);
            List<Song> l = db.Songs.AsNoTracking().Where(s=>
                   s.Artist.Contains(q) || 
                   s.TitleWithoutArticle.Contains(q)
                   ).ToList();
            if (l.Count() == 0 )
            {
                if (ql.Count() >= 2)
                {
                    string artist = ql[0];
                    string title = ql[1];
                    l = db.Songs.AsNoTracking().Where(s =>
                       s.Artist.Contains(artist) &&
                       s.TitleWithoutArticle.Contains(title)
                       ).ToList();
                }

            }
            return l;
        }

        public Song GetSong(Guid Gid)
        {
            Song p = db.Songs.AsNoTracking().FirstOrDefault(pp => pp.Gid == Gid);
            return p;
        }

        public void SaveSong(Song song)
        {
            Song dbentity = GetSong(song.Gid);
            if (dbentity == null)
            {
                //if (user.FavoriteProject!=null) db.Entry(user.FavoriteProject).State = EntityState.Unchanged;
                db.Songs.Add(song);

            }
            else
            {
                db.UpdateGraph(song);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ValidationException E = new ValidationException(ex);
                foreach (DbEntityValidationResult r in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError err in r.ValidationErrors)
                    {
                        E.FieldErrors.Add(new ValidationError(err.PropertyName, err.ErrorMessage));
                    }
                }
                throw E;
            }
            catch (Exception E)
            {
                throw E.InnerException ?? E;
            }
        }
        public void DeleteSong(Guid Gid)
        {
            Song song = GetSong(Gid);
            db.Entry(song).State = EntityState.Deleted;
            db.SaveChanges();
        }
        public IEnumerable<string> GetAllArtists(string Query)
        {
            Query = Query.ToLower().Replace("the", "").Trim();

            List<string> l = db.Songs.AsNoTracking().Where(s => s.Artist.StartsWith(Query)).Select(s => s.Artist).Distinct().ToList();
            return l;
        }
        public IEnumerable<string> GetAllSongsByArtist(string Artist, string Query)
        {
            Query = Query.ToLower().Replace("the", "").Trim();

            List<string> l = db.Songs.AsNoTracking().Where(s =>s.Artist==Artist && s.TitleWithoutArticle.StartsWith(Query)).Select(s => s.Title).Distinct().ToList();
            return l;
        }

    }
}