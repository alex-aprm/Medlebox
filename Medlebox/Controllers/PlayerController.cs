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

        public ActionResult Next(Guid id)
        {
            SongInPlaylist songin = dal.GetPlayable(id);
            if (songin == null) return HttpNotFound();
            Playlist p = dal.GetPlaylist(songin.Playlist.Gid);
            if (p == null) return HttpNotFound();
            int index = p.Songs.IndexOf(p.Songs.FirstOrDefault(s => s.Gid == songin.Gid));
            if (index < p.SongsCount - 1)
                index++;
            else index = 0;
            songin = p.Songs[index];
            return RedirectToAction("Play", new { id = songin.Gid });

            //ViewBag.SongUrl = Url.Action("mp3", "songs", new { id = songin.Song.Gid });
            //ViewBag.Song = songin;
            //ViewBag.Playlist = p;
            //return View("Play", p.Songs);
        }
        public ActionResult Prev(Guid id)
        {
            SongInPlaylist songin = dal.GetPlayable(id);
            if (songin == null) return HttpNotFound();
            Playlist p = dal.GetPlaylist(songin.Playlist.Gid);
            if (p == null) return HttpNotFound();
            int index = p.Songs.IndexOf(p.Songs.FirstOrDefault(s => s.Gid == songin.Gid));
            if (index >0)
                index--;
            else index= p.SongsCount-1;
            songin = p.Songs[index];
            return RedirectToAction("Play", new { id = songin.Gid });

            //ViewBag.SongUrl = Url.Action("mp3", "songs", new { id = songin.Song.Gid });
            //ViewBag.Song = songin;
            //ViewBag.Playlist = p;
            //return View("Play", p.Songs);
        }


    }
}
