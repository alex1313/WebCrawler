namespace WebCrawler.Controllers
{
    using System.Web.Mvc;
    using Services;

    public class HomeController : Controller
    {
        private readonly IUsersAnalyzeService _service;

        public HomeController(IUsersAnalyzeService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var model = _service.GetUsersDetails();

            return View(model);
        }
    }
}