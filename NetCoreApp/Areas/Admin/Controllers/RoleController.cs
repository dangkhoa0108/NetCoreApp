using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            return new OkObjectResult(await _roleService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return new OkObjectResult(await _roleService.GetById(id));
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            return new OkObjectResult(_roleService.GetAllPagingAsync(keyword, page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel appRole)
        {
            if (!ModelState.IsValid)
            {
                var allError = ModelState.Values.SelectMany(e => e.Errors);
                return new BadRequestObjectResult(allError);
            }

            if (appRole.Id.HasValue)
            {
                await _roleService.UpdateAsync(appRole);
            }

            await _roleService.AddAsync(appRole);
            return new OkObjectResult(appRole);
        }

        [HttpPost]
        public IActionResult GetListFunction(Guid id)
        {
            return new OkObjectResult(_roleService.GetListFunctionWithRole(id));
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermission, Guid roleId)
        {
            _roleService.SavePermission(listPermission, roleId);
            return new OkResult();
        }
    }
}