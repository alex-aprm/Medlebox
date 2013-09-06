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
        public ActionResult Test()
        {
            Response.Write("hi");
            HttpContext.ApplicationInstance.CompleteRequest();
            return new EmptyResult();
        }
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
                //ViewBag.resource = resource;
                //ViewBag.url = url + "/save/" + resource;
                response = webClient.DownloadString(url + "/save/" + resource);
                string link = url + response["#download_url"].Attr("href");
                link = link.Split(new string[] { "/mp3/" }, StringSplitOptions.RemoveEmptyEntries)[0];

                var client = new WebClient();
                
                var stream = client.OpenRead(link);
                return File(stream, "audio/mpeg");
                

               // ViewBag.SongName = SongName;
            }
            return View();
        }

    }
}
