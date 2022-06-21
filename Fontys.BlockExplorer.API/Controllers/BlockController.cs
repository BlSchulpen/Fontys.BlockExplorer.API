namespace Fontys.BlockExplorer.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BlockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
