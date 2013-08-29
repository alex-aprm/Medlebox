using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Medlebox.Controllers
{
    public class SongsAPIController : BaseAPIController
    {
        public IEnumerable<string> GetAllArtists(string q)
        {
            return dal.GetAllArtists(q);
        }
        public IEnumerable<string> GetAllSongsByArtist(string Artist, string q)
        {
            return dal.GetAllSongsByArtist(Artist,q);
        }
    }
}
