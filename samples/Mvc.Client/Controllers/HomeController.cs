using Microsoft.AspNet.Mvc;

namespace Mvc.Client.Controllers {
    public class HomeController : Controller {
        [HttpGet("~/")]
        public ActionResult Index() => View();
    }
}