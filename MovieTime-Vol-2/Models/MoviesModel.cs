using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTime_Vol_2.Models
{
    public class MoviesModel
    {
        public List<MovieModel> Movies { get; set; }
        public int Pages { get; set; }
        public int Current_Page { get; set; }
        public string Search { get; set; }
        public int FilterCatergory { get; set; }
        public List<MovieWatchlist> Watchlist { get; set; }
        public string Temp_MovieID { get; set; }
        public string SessionUser { get; set; }

    }
}