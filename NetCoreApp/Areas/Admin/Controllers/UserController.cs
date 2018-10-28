using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Authorization;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded) return View();
            return new RedirectResult("/Admin/Login/Index");
        }
        public IActionResult GetAll()
        {
            return new OkObjectResult(_userService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            return new OkObjectResult(await _userService.GetByIdAsync(id));
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            return new OkObjectResult(_userService.GetAllPagingAsync(keyword, page, pageSize));
        }
        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel appUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(e => e.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (appUserViewModel.Id == null)
            {
                await _userService.AddAsync(appUserViewModel);
            }
            else
            {
                await _userService.UpdateAsync(appUserViewModel);
            }
            return new OkObjectResult(appUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _userService.DeleteAsync(id);
            return new OkObjectResult(id);
        }
    }
}