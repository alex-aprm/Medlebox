using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using CsQuery;
using Medlebox.Models;

namespace Medlebox.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(Guid? Gid)
        {
            string SongName = "";
            Song song = dal.GetSong(Gid ?? Guid.Empty);
            if (song == null)
            {
                Playlist playlist = dal.GetPlaylist(Gid ?? Guid.Empty);
                if (playlist != null)
                    if (playlist.SongsCount > 0)
                        song = playlist.Songs.First().Song;
            }
            if (song != null) SongName = song.Name;

            if (SongName != "")
            {
                string url = "http://ololo.fm";
                WebClient webClient = new WebClient();
                webClient.QueryString.Add("query", SongName);
                CQ response = webClient.DownloadString(url + "/search/");
                string resource = response[".listen_href:first"].Attr("data-resource");
                webClient.QueryString.Clear();
                response = webClient.DownloadString(url + "/save/" + resource);
                string link = url + response["#download_url"].Attr("href");
                link = link.Split(new string[] { "/mp3/" }, StringSplitOptions.RemoveEmptyEntries)[0];

                ViewBag.SongName = link;
            }
            return View();
        }

    }
}
