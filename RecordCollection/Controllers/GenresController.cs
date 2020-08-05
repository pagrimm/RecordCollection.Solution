using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
  public class GenresController : Controller
  {
    private readonly RecordCollectionContext _db;

    public GenresController(RecordCollectionContext db)
    {
      _db = db;
    }

    public ActionResult Index(string name)
    {
      IQueryable<Genre> genreQuery = _db.Genres;
      if (!string.IsNullOrEmpty(name))
      {
        Regex search = new Regex(name, RegexOptions.IgnoreCase);
        genreQuery = genreQuery.Where(genres => search.IsMatch(genres.Name));
      }
      IEnumerable<Genre> genreList = genreQuery.ToList().OrderBy(genres =>genres.Name);
      return View(genreList);
    }

    public ActionResult Create()
    {
      ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Genre genre, int AlbumId, int ArtistId)
    {
      _db.Genres.Add(genre);
      if (AlbumId != 0)
      {
        _db.AlbumArtistGenre.Add(new AlbumArtistGenre() { AlbumId = AlbumId, GenreId = genre.GenreId });
      }
      if (ArtistId != 0)
      {
        _db.AlbumArtistGenre.Add(new AlbumArtistGenre() { ArtistId = ArtistId, GenreId = genre.GenreId });
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = genre.GenreId });
    }

    public ActionResult Details(int id)
    {
      Genre genre = _db.Genres
        .Include(genres => genres.AlbumsArtists)
        .ThenInclude(join => join.Album)
        .Include(genres => genres.AlbumsArtists)
        .ThenInclude(join => join.Artist)
        .FirstOrDefault(genres => genres.GenreId == id);
      return View(genre);
    }

    public ActionResult Edit(int id)
    {
      Genre genre = _db.Genres.FirstOrDefault(genres => genres.GenreId == id);
      return View(genre);
    }

    [HttpPost]
    public ActionResult Edit(Genre genre)
    {
      _db.Entry(genre).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = genre.GenreId });
    }

    public ActionResult Delete(int id)
    {
      Genre genre = _db.Genres.FirstOrDefault(genres => genres.GenreId == id);
      return View(genre);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Genre genre = _db.Genres.FirstOrDefault(genres => genres.GenreId == id);
      _db.Genres.Remove(genre);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddAlbum(int id)
    {
      Genre genre = _db.Genres.FirstOrDefault(genres => genres.GenreId == id);
      ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
      return View(genre);
    }

    [HttpPost]
    public ActionResult AddAlbum(Genre genre, int AlbumId)
    {
      _db.AlbumArtistGenre.Add(new AlbumArtistGenre(){ AlbumId = AlbumId, GenreId = genre.GenreId });
      return RedirectToAction("Details", new { id = genre.GenreId });
    }

    public ActionResult AddArtist(int id)
    {
      Genre genre = _db.Genres.FirstOrDefault(genres => genres.GenreId == id);
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      return View(genre);
    }

    [HttpPost]
    public ActionResult AddArtist(Genre genre, int ArtistId)
    {
      _db.AlbumArtistGenre.Add(new AlbumArtistGenre(){ ArtistId = ArtistId, GenreId = genre.GenreId });
      return RedirectToAction("Details", new { id = genre.GenreId });
    }
  }
}