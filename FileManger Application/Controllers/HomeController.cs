using Microsoft.AspNetCore.Mvc;

namespace FileManger_Application.Controllers
{
    [Route("[action]")]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
