using CoreApp.Application.Singleton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BaseController : Controller
    {
        public IServiceRegistration ServiceRegistration { get; set; }
        public BaseController()
        {
            if (ServiceRegistration == null)
            {
                ServiceRegistration=new ServiceRegistration();
            }
        }
    }
}