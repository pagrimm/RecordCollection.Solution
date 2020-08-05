using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
  public class ArtistsController : Controller
  {
    private readonly RecordCollectionContext _db;

    public ArtistsController(RecordCollectionContext db)
    {
      _db = db;
    }

    public ActionResult Index(string name)
    {
      IQueryable<Artist> artistQuery = _db.Artists;
      if (!string.IsNullOrEmpty(name))
      {
        Regex search = new Regex(name, RegexOptions.IgnoreCase);
        artistQuery = artistQuery.Where(artists => search.IsMatch(artists.Name));
      }
      IEnumerable<Artist> artistList = artistQuery.ToList().OrderBy(artists => artists.Name);
      return View(artistList);
    }

    public ActionResult Create()
    {
      ViewBag.AlbumId = new SelectList(_db.Albums; "AlbumId", "Name");
      ViewBag.GenreId = new SelectList(_db.Genres; "GenreId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Artist artist, int AlbumId, int GenreId)
    {
      _db.Artists.Add(artist);
      if (AlbumId != 0)
      {
        _db.AlbumArtistGenre.Add(new AlbumArtistGenre() { AlbumId = AlbumId, ArtistId = artist.ArtistId });
      }
      if (GenreId != 0)
      {
        _db.AlbumArtistGenre.Add(new AlbumArtistGenre() { GenreId = GenreId, ArtistId = artist.ArtistId });
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = artist.ArtistId });
    }

    public ActionResult Details(int id)
    {
      Artist artist = _db.Artists
        .Include(artists => artists.AlbumArtistGenre)
        .ThenInclude(join => join.Album)
        .Include(artists => artists.AlbumArtistGenre)
        .ThenInclude(join => join.Genre)
        .FirstOrDefault(artists => artists.ArtistId = id);
      return View(artist);
    }

    public ActionResult Edit(int id)
    {
      Artist artist = _db.Artists.FirstOrDefault(artists => artists.ArtistId == id);
      return view(artist);
    }

    public ActionResult Edit(Artist artist)
    {
      _db.Entry(artist).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id == artist.ArtistId });
    }

    public ActionResult Delete(int id)
    {
      Artist artist = _db.Artists.FirstOrDefault(artists => artist.ArtistId == id);
      return View(artist);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Artist artist = _db.Artist.FirstOrDefault(artists => artist.ArtistId == id);
      _db.Artists.Remove(artist);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddAlbum(int id)
    {
      Artist artist = _db.Artist.FirstOrDefault(artists => artist.ArtistId == id);
      ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
      return View(artist);
    }

    [HttpPost]
    public ActionResult AddAlbum(Artist artist, int AlbumId)
    {
      _db.AlbumArtistGenre.Add(new AlbumArtistGenre(){ AlbumId = AlbumId, ArtistId = artist.ArtistId });
      return RedirectToAction("Details", new { id = artist.ArtistId });
    }

    public ActionResult AddGenre(int id)
    {
      Artist artist = _db.Artist.FirstOrDefault(artists => artist.ArtistId == id);
      ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
      return View(artist);
    }

    [HttpPost]
    public ActionResult AddGenre(Artist artist, int GenreId)
    {
      _db.AlbumArtistGenre.Add(new AlbumArtistGenre(){ GenreId = GenreId, ArtistId = artist.ArtistId });
      return RedirectToAction("Details", new { id = artist.ArtistId });
    }
  }
}