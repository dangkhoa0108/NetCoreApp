using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
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