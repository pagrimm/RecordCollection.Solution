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
  }
}