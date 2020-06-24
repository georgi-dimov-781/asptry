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

namespace MovieCatalog.Views
{
    public class MoviesController : Controller
    {
        private MovieDBEntities db = new MovieDBEntities();

        // GET: Movies

        [Authorize(Roles = "Administrator")]

        public ActionResult Index(string searching, string sortByGN, int? i)
        {
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName");
           // ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName");
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortByGN) ? "ReleaseDate" : "";
            var movie = db.Movie.Include(m => m.Author).Include(m => m.Genre);

            if (!String.IsNullOrEmpty(searching))
            {
                movie = movie.Where(x => x.Title.Contains(searching)
                                       ||  searching == null);
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
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName");
            var movie = db.Movie.Include(x => x.Title).Where(m => m.AuthorID == AuthorID);
            return View(db.Movie.Where(m => m.AuthorID == AuthorID).ToList().ToPagedList(i ?? 1, 3));
        }



        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName");
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieID,Title,ReleaseDate,Description,AuthorID,GenreID")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movie.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName", movie.AuthorID);
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName", movie.AuthorID);
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName", movie.GenreID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,Title,ReleaseDate,Description,AuthorID,GenreID")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorFirstName", movie.AuthorID);
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movie.Find(id);
            db.Movie.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
