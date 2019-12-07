using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTime_Vol_2.Models
{
    public class MovieModel
    {
        public string Title { get; set; }
        public float Popularity { get; set; }
        public string Id { get; set; }
        public string Release_date { get; set; }
        public string Overview { get; set; }
        public string Poster_path { get; set; }

        public List<string> VideoLinks { get; set; }

        public List<string> VideoNames { get; set; }

        public Dictionary<string, string> MovieReview { get; set; }

        public List<int> Genre_ids { get; set; }

      
    }
}