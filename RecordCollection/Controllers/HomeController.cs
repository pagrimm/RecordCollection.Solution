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
  }
}