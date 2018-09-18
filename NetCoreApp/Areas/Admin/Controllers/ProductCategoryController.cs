using CoreApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Get Category API

        public IActionResult GetProductCategory()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        #endregion
    }
}