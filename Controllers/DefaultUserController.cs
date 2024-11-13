using Microsoft.AspNetCore.Mvc;

namespace ST10251759_PROG6212_POE.Controllers
{
    public class DefaultUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
