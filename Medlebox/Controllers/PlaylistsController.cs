using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;
using Medlebox.Models;
using Medlebox.DAL;

namespace Medlebox.Controllers
{
            [Authorize]

    public class PlaylistsController : BaseController
    {
        //
        // GET: /Playlists/

        public ActionResult Index()
        {
            List<Playlist> l = dal.GetPlaylists(dal.CurrentUser);
            return View(l);
        }

        public ActionResult Edit(Guid? id)
        {
            Playlist p = dal.GetPlaylist((Guid)id);
            if (p == null) p = new Playlist();
            return View(p);
        }

        public ActionResult Songs(Guid id)
        {
            Playlist p = dal.GetPlaylist((Guid)id);
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Playlist playlist, string SubAction, string RoleGid)
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
                        dal.SavePlaylist(playlist);
                        return RedirectToAction("Index", "Playlists", new { Back = true });
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
            return View(playlist);
        }

        public ActionResult Delete(Guid id)
        {
            Playlist p = dal.GetPlaylist((Guid)id);
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            dal.DeletePlaylist(id);
            return RedirectToAction("Index", "Playlists", new { Back = true });
        }

    }
}
