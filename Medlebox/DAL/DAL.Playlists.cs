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
        public List<Playlist> GetPlaylists()
        {
            List<Playlist> ul = db.Playlists.AsNoTracking().ToList();
            return ul;
        }
        public List<Playlist> GetPlaylists(User user)
        {
            List<Playlist> ul = db.Playlists.AsNoTracking().Where(p => p.User.Gid == user.Gid).ToList();
            return ul;
        }
        public Playlist GetPlaylist(Guid Gid)
        {
            Playlist p = db.Playlists.AsNoTracking().FirstOrDefault(pp => pp.Gid == Gid);
            if (p != null)
            {
                p.Songs = p.Songs.OrderBy(s => s.NumOrder).ToList();
            }
            return p;
        }
        public void SavePlaylist(Playlist playlist)
        {
            Playlist dbentity = GetPlaylist(playlist.Gid);
            if (dbentity == null)
            {
                //if (user.FavoriteProject!=null) db.Entry(user.FavoriteProject).State = EntityState.Unchanged;
                playlist.User = CurrentUser;
                db.Entry(CurrentUser).State = EntityState.Unchanged;
                db.Playlists.Add(playlist);

            }
            else
            {
                foreach (SongInPlaylist sp in playlist.Songs)
                {
                    sp.NumOrder = playlist.Songs.IndexOf(sp);
                }

                db.UpdateGraph(playlist, p=>p.OwnedCollection(pp=>pp.Songs));
                foreach (SongInPlaylist sp in playlist.Songs)
                {
                    db.Entry(sp.Song).State = EntityState.Unchanged;
                }
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
        public void DeletePlaylist(Guid Gid)
        {
            Playlist playlist = GetPlaylist(Gid);
            db.Entry(playlist).State = EntityState.Deleted;
            db.SaveChanges();
        }

    }
}