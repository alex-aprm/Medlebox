using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using CsQuery;

namespace Medlebox.Global
{
    public static class MP3Producer
    {
        public static string GetLinkFromOlolo(string SongName)
        {
            string url = "http://ololo.fm";
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("query", SongName);
            CQ response = webClient.DownloadString(url + "/search/");
            string resource = response[".listen_href:first"].Attr("data-resource");
            webClient.QueryString.Clear();
            response = webClient.DownloadString(url + "/save/" + resource);
            string link = url + response["#download_standart"].Find("a.btn").Attr("href");
            link = link.Split(new string[] { "/mp3/" }, StringSplitOptions.RemoveEmptyEntries)[0];


            return link;
        }
    }
}