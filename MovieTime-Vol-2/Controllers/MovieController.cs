using Microsoft.Ajax.Utilities;
using MovieTime_Vol_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MovieTime_Vol_2.Controllers
{

    public class MovieController : Controller
    {
        public static MoviesModel Movies;
        public ActionResult Index(int id = 1)
        {
            try
            {
                string username = (string)Session["username"].ToString();
                Session["username"]   = username;

            }
            catch { }
            MoviesModel ListOfMovies = new MoviesModel();
            ListOfMovies.FilterCatergory = 0;
            ListOfMovies = API_Movie.GetCurrentlyPlayingMovies(id);
            Movies = ListOfMovies;
            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }
            ViewBag.Title = "Currently Playing Movies";
            return View(Movies);
        }

        public ActionResult FilterCategory(int id)
        {

            MoviesModel ListOfMovies = new MoviesModel();
            ListOfMovies = Movies;
            ListOfMovies.FilterCatergory = id;




            if (ListOfMovies.FilterCatergory == 1)
            {
                ListOfMovies.Movies = ListOfMovies.Movies.OrderByDescending(o => o.Popularity).ToList();
            }

            else if (ListOfMovies.FilterCatergory == 2)
            {
                ListOfMovies.Movies = ListOfMovies.Movies.OrderBy(o => o.Title).ToList();
            }

            else if (ListOfMovies.FilterCatergory == 3)
            {
                ListOfMovies.Movies = ListOfMovies.Movies.OrderByDescending(o => o.Release_date).ToList();
            }

            Movies = ListOfMovies;
            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }

            return View(Movies);


        }
        public ActionResult GetTopRatedMovies(int id)
        {

            MoviesModel TopRatedMovies = new MoviesModel();
            TopRatedMovies = API_Movie.GetTopRatedMovies(id);
            Movies = TopRatedMovies;

            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }
            ViewBag.Title = "Top Rated Movies";
            return View(Movies);
        }
        public ActionResult GetUpComingMovies(int id)
        {

            MoviesModel UpcomingMovies = new MoviesModel();
            UpcomingMovies = API_Movie.GetUpComingMovies(id);
            Movies = UpcomingMovies;
            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }
            ViewBag.Title = "Upcoming Movies";
            return View(Movies);
        }
        public ActionResult GetPopularMovies(int id)
        {

            MoviesModel PopularMovies = new MoviesModel();
            PopularMovies = API_Movie.GetPopularMovies(id);
            Movies = PopularMovies;
            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }
            ViewBag.Title = "Popular Movies";
            return View(Movies);
        }
        public ActionResult GetMovieInformation(string id, string watchlist = null)
        {
            MovieModel movie = new MovieModel();

            if (watchlist == null)
            {

                foreach (var Movie in Movies.Movies)
                {
                    if (Movie.Id == id)
                    {
                        movie = API_Movie.GetSearchMovieVideos(Movie.Id);
                        Movie.MovieReview = API_Movie.GetMovieReviews(Movie.Id);
                        Movie.VideoLinks = movie.VideoLinks;
                        Movie.VideoNames = movie.VideoNames;
                        return View(Movie);
                    }
                }
            }
            else if (watchlist != null)
            {
                foreach (var Movie in Movies.Watchlist)
                {
                    if (Movie.Title == watchlist)
                    {
                        movie = API_Movie.GetSearchMovieVideos(Movie.MovieID);
                        movie.MovieReview = API_Movie.GetMovieReviews(Movie.MovieID);
                        movie.Title = Movie.Title;
                        movie.Overview = Movie.Overview;
                        movie.Popularity = float.Parse(Movie.Popularity);
                        movie.Poster_path = Movie.Poster_path;
                        movie.Release_date = Movie.Release_date;

                        return View(movie);
                    }
                }
            }


            return RedirectToAction("Index", 1);
        }



        public ActionResult Search(int id)

        {

            MoviesModel SearchMovies = new MoviesModel();


            if (Movies.Pages > Movies.Current_Page && id > 1)
            {
                SearchMovies = API_Movie.SearchMovie(Movies.Search, id);
            }
            else
            {
                SearchMovies = API_Movie.SearchMovie(Movies.Search);
            }


            Movies.Movies = SearchMovies.Movies;
            Movies.Pages = SearchMovies.Pages;
            Movies.Current_Page = SearchMovies.Current_Page;
            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }
            return View(Movies);
        }




        [HttpPost]
        public ActionResult Search(MoviesModel model)

        {

            MoviesModel SearchMovies = new MoviesModel();

            if (model.Search == null)
            {
                return View(Movies);
            }
            model.Search = model.Search.Replace(" ", "%20");

            if (model.Pages > model.Current_Page && model.Current_Page > 1)
            {
                SearchMovies = API_Movie.SearchMovie(model.Search, model.Current_Page);
            }
            else
            {
                SearchMovies = API_Movie.SearchMovie(model.Search);
            }


            Movies = SearchMovies;
            Movies.Search = model.Search;
            if (Session["username"]   != null)
            {
                Movies.Watchlist = GetMovieWatchlist();
            }
            return View(Movies);
        }



        public ActionResult AddToMovieWatchlist(string id, string title, string watchlistname, string release_date)
        {

            MovieWatchlist MovieWatchlist = new MovieWatchlist();
            var db = new MovieTimeEntities6();

            string UserID = (string)Session["username"].ToString();


            var listofMovies = db.MovieWatchlists.Where(x => x.WatchlistName.Equals(watchlistname)).ToList();

            if (listofMovies.Count == 0)
            {
                return RedirectToAction("Index", "Movie");
            }
            Random rnd = new Random();

            MovieWatchlist.ID = rnd.Next(1, 10000000).ToString();

            List<MovieWatchlist> check = db.MovieWatchlists.Where(m => m.ID.Equals(MovieWatchlist.ID) && m.UserID.Equals(UserID)).ToList();
            while (check.Count != 0)
            {
                MovieWatchlist.ID = rnd.Next(1, 10000000).ToString();
                check = db.MovieWatchlists.Where(m => m.ID.Equals(MovieWatchlist.ID) && m.UserID.Equals(UserID)).ToList();
            }



            foreach (var movie in Movies.Movies)
            {
                if (movie.Title.Equals(title) && movie.Release_date.Equals(release_date))
                {
                    MovieWatchlist.UserID = UserID;
                    MovieWatchlist.WatchlistName = watchlistname;
                    MovieWatchlist.Overview = movie.Overview;
                    MovieWatchlist.Popularity = movie.Popularity.ToString();
                    MovieWatchlist.Release_date = movie.Release_date;
                    MovieWatchlist.Poster_path = movie.Poster_path;
                    MovieWatchlist.Title = movie.Title;
                    MovieWatchlist.MovieID = movie.Id;
                    break;

                }
            }
            db.Entry(MovieWatchlist).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            Movies.Search = null;
            MoviesModel temp = new MoviesModel();
            temp = Movies;
            temp.Watchlist = GetMovieWatchlist();

            return View(temp);



        }
        public ActionResult CreateMovieWatchlist()
        {
            return View(new MovieWatchlist());
        }


        [HttpPost]
        public ActionResult CreateMovieWatchlist(MovieWatchlist model)
        {
            Random rnd = new Random();

            string str = (string)Session["username"].ToString();
            var db = new MovieTimeEntities6();
            // int count = db.MovieWatchlists.Count();
            if (str == null)
            {
                return View();
            }

            MovieWatchlist MovieWatchlist = new MovieWatchlist();

            MovieWatchlist.ID = rnd.Next(1, 10000000).ToString();
            List<MovieWatchlist> check = db.MovieWatchlists.Where(m => m.ID.Equals(MovieWatchlist.ID)).ToList();
            while (check.Count != 0)
            {
                MovieWatchlist.ID = rnd.Next(1, 10000000).ToString();
                check = db.MovieWatchlists.Where(m => m.ID.Equals(MovieWatchlist.ID)).ToList();
            }


            MovieWatchlist.UserID = str;
            check = db.MovieWatchlists.Where(m => m.WatchlistName.Equals(model.WatchlistName) && m.UserID.Equals(str)).ToList();
            if (check.Count == 0)
            {
                MovieWatchlist.WatchlistName = model.WatchlistName;
                db.Entry(MovieWatchlist).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

            }
            else
            {
                ViewBag.Message = "MovieWatchlist Name Exitst Already!";
                return View(new MovieWatchlist());
            }


            return RedirectToAction("index", "Movie");
        }



        public List<MovieWatchlist> GetMovieWatchlist()
        {
            string str = (string)Session["username"].ToString();
            var db = new MovieTimeEntities6();

            List<MovieWatchlist> x = db.MovieWatchlists.Where(m => m.UserID.Equals(str)).ToList().DistinctBy(m => m.WatchlistName).ToList();
            return x;
        }
        public ActionResult ShowMovieWatchlist(string id)
        {
            string str = Session["username"].ToString();
            var db = new MovieTimeEntities6();
            List<MovieWatchlist> UniqueNames = GetMovieWatchlist();
            List<MovieWatchlist> x = db.MovieWatchlists.Where(m => m.UserID.Equals(str) && m.WatchlistName.Equals(id)).ToList();

            Movies.Watchlist = x;
            if (x == null)
            {
                return RedirectToAction("Index", "Movie");
            }
            ViewBag.Message = "Logged";

            return View(x);

        }

        public ActionResult RemoveMovieFromYourList(string id, string title, string watchlistname, string release_date)
        {
            var db = new MovieTimeEntities6();
            string str = (string)Session["username"].ToString();
            MovieWatchlist MovieWatchlist = db.MovieWatchlists.Where(movie => movie.Title.Equals(title) && movie.UserID.Equals(str) && movie.Release_date.Equals(release_date) && movie.WatchlistName.Equals(watchlistname)).FirstOrDefault();
            db.Entry(MovieWatchlist).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Redirect($"~/Movie/ShowMovieWatchlist/{watchlistname}");

        }
    }
}