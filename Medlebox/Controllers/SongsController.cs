using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;
using Medlebox.DAL;

namespace Medlebox.Controllers
{
    [Authorize]

    public class SongsController : BaseController
    {
        //
        // GET: /Playlists/

        public ActionResult Index(string q)
        {
            if (q == null)
                return View(new List<Song>());
            List<Song> l = dal.GetSongsBySearch(q) ;
            return View(l);
        }

        public ActionResult Create()
        {
            Song s = new Song();
            return View("Edit", s);
        }

        public ActionResult Edit(Guid? id)
        {
            Song s = dal.GetSong((Guid)id);
            if (s == null) s = new Song();
            return View(s);
        }

        [HttpPost]
        public ActionResult Edit(Song song, string SubAction, string RoleGid)
        {
            if (SubAction != "") //Если промежуточное действие с моделью
            {
                //user = ProcessSubAction(user, SubAction, RoleGid);
            }
            else //Если сохраняем модель
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        dal.SaveSong(song);
                        return RedirectToAction("Index", "Songs", new { Back = true });
                    }
                    catch (ValidationException ex)
                    {
                        foreach (ValidationError err in ex.FieldErrors)
                        {
                            ModelState.AddModelError(err.Field, err.Error);
                        }
                    }
                }

            }
            return View(song);
        }

        public ActionResult Delete(Guid id)
        {
            Song s = dal.GetSong((Guid)id);
            return View(s);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            dal.DeleteSong(id);
            return RedirectToAction("Index", "Songs", new { Back = true });
        }

    }
}
