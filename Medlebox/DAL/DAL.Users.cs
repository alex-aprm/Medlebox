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
                List<ValidationError> errors = new List<ValidationError>();
                User found = GetUser(user.Email);
                if (found != null)
                    errors.Add(new ValidationError("Email", "Пользователь с таким адресом уже есть"));

                if (user.PasswordChange == null)
                    errors.Add( new ValidationError("PasswordChange", "Пароль не может быть пустым"));
                if (user.PasswordChange != user.PasswordConfirm)
                {
                    errors.Add(new ValidationError("PasswordChange", " "));
                    errors.Add(new ValidationError("PasswordConfirm", "Введенные пароли не совпадают"));
                }
                if (errors.Count() > 0)
                    throw new ValidationException(errors);
                user.SetPwdHash();
                db.Users.Add(user);
            }
            else
            {
                List<ValidationError> errors = new List<ValidationError>();
                if (user.Nickname == null)
                    errors.Add(new ValidationError("NickName", "Необходимо указать имя"));
                if (user.MusicSource==MusicSource.NotSet)
                   errors.Add(new ValidationError("MusicSource", "Необходимо выбрать источник музыки"));
                if (user.Gid != CurrentUser.Gid)
                   errors.Add(new ValidationError("", "Нельзя изменить чужой профиль."));
                if (errors.Count() > 0)
                    throw new ValidationException(errors);

                user.Email = dbentity.Email;

                if (user.PasswordChange != null)
                {
                    User found = GetUser(user.Email);
                    bool UserValid = false;
                    if (found != null)
                    {
                        user.PwdHash = found.PwdHash;
                        UserValid = user.TryLogin();
                    }
                    if (!UserValid)
                        errors.Add(new ValidationError("Password", "Неверный пароль"));
                    if (user.PasswordChange != user.PasswordConfirm)
                    {
                        errors.Add(new ValidationError("PasswordChange", " "));
                        errors.Add(new ValidationError("PasswordConfirm", "Введенные пароли не совпадают"));
                    }
                    if (errors.Count() > 0)
                        throw new ValidationException(errors);
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

	}
}