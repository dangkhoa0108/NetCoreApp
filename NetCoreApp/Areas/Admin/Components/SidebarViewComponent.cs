using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreApp.Application.Singleton;
using CoreApp.Application.ViewModels.System;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Extensions;

namespace NetCoreApp.Areas.Admin.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IServiceRegistration _serviceRegistration;
        public SidebarViewComponent(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal) User).GetSpecificClaim("Roles");
            List<FunctionViewModel> function;
            if (roles.Split(";").Contains(CommonConstants.Admin))
            {
                function = await _serviceRegistration.FunctionService.GetAll(string.Empty);
            }
            else
            {
                function= new List<FunctionViewModel>();
            }
            return View(function);
        }
    }
}
