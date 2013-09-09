using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medlebox.Models;

namespace Medlebox.Controllers
{
    public class PlayerController : BaseController
    {
        //
        // GET: /Player/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Play(Guid? id)
        {
            SongInPlaylist songin = dal.GetPlayable(id ?? Guid.Empty);
            ViewBag.SongUrl = Url.Action("mp3", "songs", new { id = songin.Song.Gid });
            Playlist p = dal.GetPlaylist(songin.Playlist.Gid);

            ViewBag.Song = songin;
            ViewBag.Playlist = p;
            return View(p.Songs);
        }


    }
}
