using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
  public class AlbumsController : Controller
  {
  
    private readonly RecordCollectionContext _db;

    public AlbumsController(RecordCollectionContext db)
    {
      _db = db;
    }

    public ActionResult Index(string name)
    {
      IQueryable<Album> albumQuery = _db.Albums;
      if (!string.IsNullOrEmpty(name))
      {
        Regex search = new Regex(name, RegexOptions.IgnoreCase);
        albumQuery = albumQuery.Where(albums => search.IsMatch(albums.Name));
      }
      IEnumerable<Album> albumList = albumQuery.ToList().OrderBy(albums => albums.Name);
      return View(albumList);
    }

    public ActionResult Create()
    {
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Album albums, int ArtistId, int GenreId)
    {
      _db.Albums.Add(albums);
      if (ArtistId != 0)
      {
        _db.AlbumArtistGenre.Add(new AlbumArtistGenre() { ArtistId = ArtistId, AlbumId = albums.AlbumId });
      }
      if (GenreId != 0)
      {
        _db.AlbumArtistGenre.Add(new AlbumArtistGenre() { GenreId = GenreId, AlbumId = albums.AlbumId });
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = albums.AlbumId });
    }

    public ActionResult Details(int id)
    {
      Album album = _db.Albums
        .Include(albums => albums.ArtistsGenres)
        .ThenInclude(join => join.Artist)
        .Include(albums => albums.ArtistsGenres)
        .ThenInclude(join => join.Genre)
        .First(albums => albums.AlbumId == id);
      ViewBag.ArtistCount = _db.AlbumArtistGenre.Where(join => join.AlbumId == id).Where(join => join.ArtistId != null).Count();
      ViewBag.GenreCount = _db.AlbumArtistGenre.Where(join => join.AlbumId == id).Where(join => join.GenreId != null).Count();
      return View(album);  
    }

    public ActionResult Edit(int id)
    {
      Album model = _db.Albums.FirstOrDefault(album => album.AlbumId == id);
      return View(model);
    }

    [HttpPost]
    public ActionResult Edit(Album album)
    {
      _db.Entry(album).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = album.AlbumId });
    }

    public ActionResult Delete(int id)
    {
      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == id);
      return View (thisAlbum);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Album thisAlbum = _db.Albums.FirstOrDefault(album => album.AlbumId == id);
      _db.Albums.Remove(thisAlbum);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddArtist(int id)
    {
      Album model = _db.Albums.FirstOrDefault(album => album.AlbumId == id);
      ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
      return View(model);
    }
    
    [HttpPost]
    public ActionResult AddArtist(Album album, int artistId)
    {
      _db.AlbumArtistGenre.Add(new AlbumArtistGenre(){AlbumId = album.AlbumId, ArtistId = artistId});
      return RedirectToAction("Details", new {id = album.AlbumId});
    }
    
    public ActionResult AddGenre(int id)
    {
      Album model = _db.Albums.FirstOrDefault(album => album.AlbumId == id);
      ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
      return View(model);
    }
    
    [HttpPost]
    public ActionResult AddGenre(Album album, int genreId)
    {
      _db.AlbumArtistGenre.Add(new AlbumArtistGenre(){AlbumId = album.AlbumId, GenreId = genreId});
      return RedirectToAction("Details", new {id = album.AlbumId});
    }
  }
}