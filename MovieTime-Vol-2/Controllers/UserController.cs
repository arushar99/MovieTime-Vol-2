using MovieTime_Vol_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MovieTime_Vol_2.Controllers
{
    public class UserController : Controller
    {
        private static UserTable User;
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static string HashPassword(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
      
        
        // GET: User
        public ActionResult Login()
        {
            return View(new UserTable());
        }

        [HttpPost]
        public ActionResult Login(UserTable model)
        {
            var db = new MovieTimeEntities();
            User = db.UserTables.Find(model.UserID);
            if (User != null)
            {
                if (model.Password != null)
                {
                    if (User.Password == model.Password)
                    {
                        HttpCookie cookie = new HttpCookie("Cookie");
                        cookie.Value = model.UserID;
                        cookie.Expires = DateTime.Now.AddSeconds(10000);
                        //System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                        Session["username"] = model.UserID;
                        ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "Movie");
                    }
                    else
                    {
                        ViewBag.Message = "Incorrect Password!";
                        return View();
                    }
                }

                else
                {
                    ViewBag.Message = "Incorrect Password!";
                    return View();
                }
            }
            ViewBag.Message = "Username doesn't exist!";
            return this.View();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUP(UserTable model)
        {
            MovieTimeEntities db = new MovieTimeEntities();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = db.UserTables.Find(model.UserID);
                    if (user == null)
                    {
                        if (model.Fname == null)
                        {
                            model.Fname = "none";
                        }
                        if (model.Lname == null)
                        {
                            model.Lname = "none";
                        }
                        if(model.DOB == null)
                        {
                            model.DOB = "none";
                        }

                       
                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();                   
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        ViewBag.Message = "Username exists already!";
                        return this.View();
                    }
                }
                return this.View();
            }
            catch
            {
                return View("Error");
            }
        }
        public ActionResult SignOut()
        {
            if (Session["username"] != null)
            {
                Session["username"] = null;
            }
            return RedirectToAction("Index", "Movie");
        }
    }
}

