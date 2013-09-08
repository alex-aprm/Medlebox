using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CsQuery;

namespace Medlebox.Study
{
    class Program
    {
        static void Main(string[] args)
        {
            Downloader d = new Downloader();
            d.Download("Metallica - Unforgiven");
        }
    }

    public class Downloader
    {
        public void Download(string SongName)
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

           //client.DownloadDataTaskAsync
            byte[] data = client.DownloadData(link);
            Console.Write(data.Length);
            Console.ReadLine();
            //HttpContext.ApplicationInstance.CompleteRequest();
            //return new EmptyResult();
        }
    }
}
