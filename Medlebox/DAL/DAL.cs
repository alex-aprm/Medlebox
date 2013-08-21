
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
                db.UpdateGraph(user);
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
        
        
        public void Dispose()
        {
            this.db.Dispose();
        }
    }

 }