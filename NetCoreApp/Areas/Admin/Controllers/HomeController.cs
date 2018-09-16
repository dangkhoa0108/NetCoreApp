using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Test
            //var email = User.GetSpecificClaim("Email");
            return View();
        }
    }
}