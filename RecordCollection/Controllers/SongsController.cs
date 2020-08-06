using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
  public class SongsController : Controller
  {
    private readonly RecordCollectionContext _db;
    public SongsController(RecordCollectionContext db)
    {
      _db = db;
    }

    public ActionResult Index(string name)
    {
      IQueryable<Song> songQuery = _db.Songs;
      if (!string.IsNullOrEmpty(name))
      {
        Regex search = new Regex(name, RegexOptions.IgnoreCase);
        songQuery = songQuery.Where(songs => search.IsMatch(songs.Name));
      }
      IEnumerable<Song> songList = songQuery.ToList().OrderBy(songs => songs.TrackNumber);
      return View(songList);
    }

    public ActionResult Create()
    {
      ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Song song)
    {
      _db.Songs.Add(song);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = song.SongId });
    }

    public ActionResult Details(int id)
    {
      Song song = _db.Songs
        .Include(songs => songs.Album)
        .ThenInclude(join => join.ArtistsGenres)
        .ThenInclude(albums => albums.Artist)
        .First(songs => songs.SongId == id);
      return View(song);
    }

    public ActionResult Edit(int id)
    {
      Song song = _db.Songs.First(songs => songs.SongId == id);
      ViewBag.AlbumId = new SelectList(_db.Albums, "AlbumId", "Name");
      return View(song);
    }

    [HttpPost]
    public ActionResult Edit(Song song)
    {
      _db.Entry(song).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = song.SongId });
    }

    public ActionResult Delete(int id)
    {
      Song song = _db.Songs.First(songs => songs.SongId == id);
      return View(song);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Song song = _db.Songs.First(songs => songs.SongId == id);
      _db.Songs.Remove(song);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}