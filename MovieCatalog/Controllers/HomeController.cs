using MovieCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieCatalog.Models;
using PagedList.Mvc;
using PagedList;

namespace MovieCatalog.Controllers
{
    public class HomeController : Controller
    {
        private MovieDBEntities db = new MovieDBEntities();


        public ActionResult Index(string searching, string sortByGN, int? i)
        {
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName");
           // ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName");
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortByGN) ? "ReleaseDate" : "";
            var movie = db.Movie.Include(m => m.Author).Include(m => m.Genre);

            if (!String.IsNullOrEmpty(searching))
            {
                movie = movie.Where(x => x.Title.Contains(searching)
                                       || searching == null);
            }
            switch (sortByGN)
            {
                case "ReleaseDate":
                    movie = movie.OrderByDescending(s => s.ReleaseDate);
                    break;

                default:
                    movie = movie.OrderBy(s => s.ReleaseDate);
                    break;


            }
            return View(movie.ToList().ToPagedList(i ?? 1, 3));
        }

        [HttpPost]
        public ActionResult Index(int AuthorID, int? i)
        {
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName");
            var movie = db.Movie.Include(x => x.Title).Where(m => m.AuthorID == AuthorID);
            return View(db.Movie.Where(m => m.AuthorID == AuthorID).ToList().ToPagedList(i ?? 1, 3));
        }

        [Authorize(Roles = "User, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}