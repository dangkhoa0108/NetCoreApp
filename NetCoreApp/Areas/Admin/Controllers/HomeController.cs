using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            //Test
            //var email = User.GetSpecificClaim("Email");
            return View();
        }
    }
}