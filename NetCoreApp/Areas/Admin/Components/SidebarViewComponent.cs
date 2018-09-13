using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Extensions;

namespace NetCoreApp.Areas.Admin.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;
        public SidebarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal) User).GetSpecificClaim("Roles");
            List<FunctionViewModel> function;
            if (roles.Split(";").Contains(CommonConstants.Admin))
            {
                function = await _functionService.GetAll();
            }
            else
            {
                function= new List<FunctionViewModel>();
            }
            return View(function);
        }
    }
}
