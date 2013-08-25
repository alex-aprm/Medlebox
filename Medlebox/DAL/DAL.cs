
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
    public  class MedleboxDAL
    {
        public User CurrentUser { get; set; }
        MedleboxDB db;
        public MedleboxDAL(string WeathertopConnection)
        {
            this.db = new MedleboxDB(WeathertopConnection);
            //God.GiveMeMaps();
        }

        #region Users
        public List<User> GetUsers()
        {
            List<User> ul = db.Users.AsNoTracking().OrderBy(r => r.Nickname).ThenBy(o => o.Email).ToList();
            return ul;
        }
        public User GetUser(Guid Gid)
        {
            return db.Users.AsNoTracking().FirstOrDefault(s => s.Gid == Gid);
        }
        public User GetUser(string Email)
        {
            return db.Users.AsNoTracking().FirstOrDefault(s => s.Email == Email);
        }
        public void SaveUser(User user)
        {
            User dbentity = GetUser(user.Gid);
            if (dbentity == null)
            {
                //if (user.FavoriteProject!=null) db.Entry(user.FavoriteProject).State = EntityState.Unchanged;
                db.Users.Add(user);
            }
            else
            {
                if (user.Gid != CurrentUser.Gid)
                    throw new ValidationException(new List<ValidationError> { new ValidationError("","Нельзя изменить чужой профиль.") });
                user.Email = dbentity.Email;
                if (user.Password != null)
                {
                    if (user.Password != user.PasswordConfirm)
                        throw new ValidationException(new List<ValidationError> { new ValidationError("PasswordConfirm", "Введенные пароли не совпадают") });
                    user.SetPwdHash();
                }
                user.PwdHash = user.PwdHash??dbentity.PwdHash;
                db.UpdateGraph(user);
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
        public void DeleteUser(Guid Gid)
        {
            User user = GetUser(Gid);
            db.Entry(user).State = EntityState.Deleted;
            db.SaveChanges();
        }
        #endregion

        #region Playlists
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
                db.UpdateGraph(playlist);
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

        #endregion


        #region Songs
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
            List<string> l = db.Songs.AsNoTracking().Where(s=>s.Artist.StartsWith(Query)).Select(s => s.Artist).Distinct().ToList();
            return l;
        }
        #endregion
        
        
        public void Dispose()
        {
            this.db.Dispose();
        }
    }

 }