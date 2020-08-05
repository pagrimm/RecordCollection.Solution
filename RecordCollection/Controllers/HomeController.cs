using Microsoft.AspNetCore.Mvc;

namespace RecordCollection.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Index(string searchOption, string searchString)
    {
      if (searchOption = "albums")
      {
        return RedirectToAction("Index", "Albums", new {name = searchString});
      }
      else if (searchOption = "artists")
      {
        return RedirectToAction("Index", "Artists", new {name = searchString});
      }
      else
      {
        return RedirectToAction("Index", "Genres", new {name = searchString});
      }
    }
  }
}