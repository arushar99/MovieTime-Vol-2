using MovieTime_Vol_2.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieTime_Vol_2
{
    public class API_Movie
    {




        public static string Key = "2db757f12937fd1e95bc71dbeab7681d";
        public static string URL = "https://api.themoviedb.org/3/movie/";
        public static string SEARCH_MOVIE_URL = "https://api.themoviedb.org/3/search/";
        public static string IMAGE_URL = "https://image.tmdb.org/t/p/w500/";

        public static async Task<string> API_CALL(string URL)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    //HTTP GET
                    var responseTask = client.GetAsync(URL);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        string str = readTask.Result;
                        return str;


                    }
                }
            }
            catch
            {

            }
            return "NULL";
        }
        public static async Task<string> Currently_Playing(int page)
        {
            string Movie_URL = $"{URL}now_playing?api_key={Key}&language=en-US&page={page.ToString()}";
            string str = await API_CALL(Movie_URL);

            return str;
        }
        public static async Task<string> TopRatedMovies(int page)
        {
            string Movie_URL = $"{URL}top_rated?api_key={Key}&language=en-US&page={page.ToString()}";
            string str = await API_CALL(Movie_URL);

            return str;
        }

        public static async Task<string> UpComingMovies(int page)
        {
            string Movie_URL = $"{URL}upcoming?api_key={Key}&language=en-US&page={page.ToString()}";
            string str = await API_CALL(Movie_URL);

            return str;

        }
        public static async Task<string> PopularMovies(int page)
        {
            string Movie_URL = $"{URL}popular?api_key={Key}&language=en-US&page={page.ToString()}";
            string str = await API_CALL(Movie_URL);

            return str;


        }

        public static MoviesModel StringToModel(string movies_str)
        {

            string[] BreakDownMoviesByPopularity = movies_str.Split(new string[] { "\"popu", "\"tit", "release", "\"over", "\"poster", "total_" }, StringSplitOptions.RemoveEmptyEntries);
            MoviesModel Movies = new MoviesModel();
            List<MovieModel> ListOfMovies = new List<MovieModel>();
            MovieModel Movie = new MovieModel();
            List<int> genres = new List<int>();
            for (int i = 0; i < BreakDownMoviesByPopularity.Length; i++)
            {


                if (BreakDownMoviesByPopularity[i].Contains("larity\":"))
                {
                    string str = BreakDownMoviesByPopularity[i];
                    int last_index = str.IndexOf(",");
                    int first_index = str.IndexOf(":");
                    if (first_index != -1 && last_index != -1)
                    {
                        string _No = str.Substring(first_index, last_index - first_index);
                        //_No = _No.Replace(":", "");
                        _No = Remove_Unnessary(_No);
                        if (_No != null)
                        {
                            Movie.Popularity = float.Parse(_No);
                        }
                    }
                }
                else if (BreakDownMoviesByPopularity[i].Contains("le\":") && !BreakDownMoviesByPopularity[i].Contains("path\":"))
                {
                    string str = BreakDownMoviesByPopularity[i];
                    str = str.Replace("\"", "");
                    int last_index = str.IndexOf(",");
                    int first_index = str.IndexOf(":");
                    if (first_index != -1 && last_index != -1)
                    {
                        string _title = str.Substring(first_index, last_index - first_index);
                        //_title = _title.Replace(":", "");
                        _title = Remove_Unnessary(_title);
                        if (_title != null)
                        {
                            Movie.Title = _title;
                        }
                    }
                }
                else if (BreakDownMoviesByPopularity[i].Contains("_date\":"))
                {
                    string str = BreakDownMoviesByPopularity[i];
                    str = str.Replace("\"", "");
                    str = str.Replace("}", "");
                    int last_index = str.IndexOf(",");
                    if (last_index == -1)
                    {
                        last_index = str.IndexOf("]");
                    }
                    int first_index = str.IndexOf(":");
                    if (first_index != -1 && last_index != -1)
                    {
                        string _release_date = str.Substring(first_index, last_index - first_index);
                        //_release_date = _release_date.Replace(":", "");
                        _release_date = Remove_Unnessary(_release_date);
                        if (_release_date != null)
                        {
                            Movie.Release_date = _release_date;
                        }
                       
                        ListOfMovies.Add(Movie);
                        Movie = new MovieModel();
                    }
                }
                else if (BreakDownMoviesByPopularity[i].Contains("view\":"))
                {
                    string str = BreakDownMoviesByPopularity[i];
                    str = str.Replace("\"", "");
                    str = str.Replace("}", "");
                    int last_index = str.LastIndexOf(",");
                    int first_index = str.IndexOf(":");
                    if (first_index != -1 && last_index != -1)
                    {
                        string _overview = str.Substring(first_index, last_index - first_index);
                        //_overview = _overview.Replace(":", "");
                        _overview = Remove_Unnessary(_overview);
                        if (_overview != null)
                        {
                            Movie.Overview = _overview;
                        }
                    }
                }

                else if (BreakDownMoviesByPopularity[i].Contains("path\":"))
                {

                    string str = BreakDownMoviesByPopularity[i];

                    str = str.Replace("\"", "");
                    str = str.Replace("}", "");
                    int last_index = str.IndexOf(",");
                    int first_index = str.IndexOf(":");
                    if (first_index != -1 && last_index != -1)
                    {
                        string _totalpages = str.Substring(first_index, last_index - first_index);
                        _totalpages = Remove_Unnessary(_totalpages);
                        if (_totalpages != null)
                        {
                            Movie.Poster_path = IMAGE_URL + _totalpages;
                        }
                    }
                    try
                    {
                        str = str.Substring(last_index);
                        str = str.Remove(0, 1);
                        last_index = str.IndexOf(",");
                        first_index = str.IndexOf(":");
                        if (first_index != -1 && last_index != -1)
                        {
                            string _id = str.Substring(first_index, last_index - first_index);
                            _id = Remove_Unnessary(_id);
                            if (_id != null)
                            {
                                Movie.Id = _id;
                            }
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        str = str.Substring(last_index);
                        string[] get_Genres= str.Split(new string[] { ",genre"}, StringSplitOptions.RemoveEmptyEntries);
                        
                        for(int j = 0; j < get_Genres.Length; j++)
                        {
                            if (get_Genres[j].Contains("_ids"))
                            {
                               
                                string[] temp = get_Genres[j].Split(new string[] { ":", ",", "[","]" }, StringSplitOptions.RemoveEmptyEntries);
                                for (int k = 0; k < temp.Length; k++)
                                {
                                    try
                                    {
                                        int no = int.Parse(temp[k]);
                                        genres.Add(no);
                                    }
                                    catch { }
                                }


                            }
                        }
                        Movie.Genre_ids = genres;
                        genres = new List<int>();
                        
                       
                    }
                    catch { }
                }
                else if (BreakDownMoviesByPopularity[i].Contains("pages\":"))
                {
                    string str = BreakDownMoviesByPopularity[i];

                    str = str.Replace("\"", "");
                    str = str.Replace("}", "");

                    int first_index = str.IndexOf(":");
                    if (str.Contains("results"))
                    {
                        int last_index = str.IndexOf(",");
                        if (first_index != -1 && last_index != -1)
                        {
                            string _totalpages = str.Substring(first_index, last_index - first_index);
                          
                            _totalpages = Remove_Unnessary(_totalpages);
                            if (_totalpages != null)
                            {
                                Movies.Pages = Convert.ToInt32(_totalpages);
                            }
                        }

                    }
                    else if (first_index != -1)
                    {
                        string _totalpages = str.Substring(first_index, str.Length - first_index);
                        _totalpages = Remove_Unnessary(_totalpages);
                      
                        if (_totalpages != null)
                        {
                            Movies.Pages = Convert.ToInt32(_totalpages);
                        }
                    }
                }

            }
            Movies.Movies = ListOfMovies;
            return Movies;
        }
        public static MoviesModel GetCurrentlyPlayingMovies(int page = 1)
        {
            string movies_str = Currently_Playing(page).Result;
            if (movies_str != "NULL")
            {
                MoviesModel Movies = StringToModel(movies_str);
                Movies.Current_Page = page;
                return Movies;
            }
            return new MoviesModel();



        }
        public static MoviesModel GetTopRatedMovies(int page = 1)
        {
            string movies_str = TopRatedMovies(page).Result;
            MoviesModel Movies = StringToModel(movies_str);
            Movies.Current_Page = page;
            return Movies;
        }

        public static MoviesModel GetPopularMovies(int page = 1)
        {
            string movies_str = PopularMovies(page).Result;
            MoviesModel Movies = StringToModel(movies_str);
            Movies.Current_Page = page;
            return Movies;
        }

        public static MoviesModel GetUpComingMovies(int page = 1)
        {
            string movies_str = UpComingMovies(page).Result;
            MoviesModel Movies = StringToModel(movies_str);
            Movies.Current_Page = page;
            return Movies;
        }
        public static async Task<string> SearchKeyWord(string movie_str, int page = 1)
        {

            string Movie_URL = $"{SEARCH_MOVIE_URL}movie?api_key={Key}&language=en-US&page={page.ToString()}&query={movie_str}";
            string str = await API_CALL(Movie_URL);

            return str;

        }

        public static MoviesModel SearchMovie(string str, int page = 1)
        {
            string movie_str = SearchKeyWord(str, page).Result;
            MoviesModel Movies = new MoviesModel();
            if (movie_str != "NULL")
            {

                Movies = StringToModel(movie_str);
            }

            Movies.Current_Page = page;
            return Movies;
        }


        public static string Remove_Unnessary(string str)
        {
            while (str.Contains("/") || str.Contains("\\") || str.Contains("{") || str.Contains("}") || str.Contains("[") || str.Contains("]") || str.Contains(":") || str.Contains(";") || str.Contains("\"") || str.Contains("rn"))
            {
                str = str.Replace("rn", "");
                str = str.Replace("\\n", "");
                str = str.Replace("\\r", "");
                str = str.Replace("\\t", "");
                str = str.Replace("/", "");
                str = str.Replace("\\", "");
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                str = str.Replace("]", "");
                str = str.Replace("[", "");
                
                str = str.Replace(":", "");
                str = str.Replace("\"", "");
                str = str.Replace(";", "");
            }

            if (str.Contains("https"))
            {
                str = str.Replace("https", "https:");
            }
            return str;
        }

        public static MovieModel FindURLS(string movies_str, MovieModel Movie)
        {
            string[] BreakDownStr= movies_str.Split(new string[] { "\"US\",","\"na"}, StringSplitOptions.RemoveEmptyEntries);
            List<string> VideoLinks = new List<string>();
            List<string> VideoNames = new List<string>();

            // MovieModel Movie = new MovieModel();
            for (int i = 0; i < BreakDownStr.Length; i++)
            {
                
                if (BreakDownStr[i].Contains("key"))
                {
                    string str = BreakDownStr[i];
                    str = str.Replace("\\", "");

                    int first = str.IndexOf(":");
                    int last = str.IndexOf(",");

                    if (first != -1 && last != -1)
                    {
                        string _key = str.Substring(first, last - first);
                        
                        _key= Remove_Unnessary(_key);
                        if (_key != null)
                        {
                               VideoLinks.Add(_key);
                            
                        }
                    }

                }
                else if (BreakDownStr[i].Contains("me\":"))
                {
                    string str = BreakDownStr[i];
                    str = str.Replace("\\", "");

                    int first = str.IndexOf(":");
                    int last = str.IndexOf(",");

                    if (first != -1 && last != -1)
                    {
                        string _name = str.Substring(first, last - first);

                        _name = Remove_Unnessary(_name);
                        if (_name != null)
                        {
                            VideoNames.Add(_name);
                           
                        }
                    }
                }
            }
            Movie.VideoLinks = VideoLinks;
            Movie.VideoNames = VideoNames;
            return Movie;
        }

        public static Dictionary<string, string> FindReviews (string url)
        {
            string[] BreakDownStr = url.Split(new string[] { "{\"au", ",\"con", ",\"id"}, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> Reviews = new Dictionary<string,string>();
            string temp = "";
            for (int i = 0; i < BreakDownStr.Length; i++)
            {
                
                if (BreakDownStr[i].Contains("thor\""))
                {
                    
                    string str = BreakDownStr[i];
                    str = str.Replace("\\", "");

                    int first = str.IndexOf(":");
                    int last = str.LastIndexOf("\"");
                    if (first != -1 && last != -1)
                    {
                        string _name = str.Substring(first, last - first);

                        _name = Remove_Unnessary(_name);
                        if (_name != null)
                        {
                            Reviews[_name] = "";
                            temp = _name;

                        }
                    }
                }
                if (BreakDownStr[i].Contains("tent\""))
                {

                    string str = BreakDownStr[i];
                    str = str.Replace("\\", "");
                    int first = str.IndexOf(":");
                    int last = str.LastIndexOf("\"");
                    if (first != -1 && last != -1)
                    {
                        string _review = str.Substring(first, last - first);

                        _review = Remove_Unnessary(_review);
                        if (_review != null)
                        {
                            Reviews[temp] = _review;
                            temp = "";

                        }
                    }
                }

            }

            return Reviews;
        }

        
        public static async Task<string> SearchMovieVideo(string id)
        {
            string Movie_URL = $"{URL}{id}/videos?api_key={Key}&language=en-US";
            string str = await API_CALL(Movie_URL);

            return str;

        }


        public static MovieModel GetSearchMovieVideos(string str)
        {
            string movie_str = SearchMovieVideo(str).Result;

            MovieModel Movie = new MovieModel();
            
            if (movie_str != "NULL")
            {
                Movie = FindURLS(movie_str, Movie);
            }

           
            return Movie;
        }

        public static async Task<string> SearchMovieReviews(string id)
        {
            string Movie_URL = $"{URL}{id}/reviews?api_key={Key}&language=en-US";
            string str = await API_CALL(Movie_URL);

            return str;
        }

        public static Dictionary<string, string> GetMovieReviews(string str)
        {
            string movie_str = SearchMovieReviews(str).Result;

            Dictionary<string, string> Reviews = new Dictionary<string, string>();

            if (movie_str != "NULL")
            {
                    Reviews = FindReviews(movie_str);
            }


            return Reviews;
        }
    }

}