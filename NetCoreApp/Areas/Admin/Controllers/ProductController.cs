using CoreApp.Application.Singleton;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Get API Data Ajax
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = ServiceRegistration.ProductService.GetAll();
            return new OkObjectResult(model);
        }

        /// <summary>
        /// Return Model Paging API
        /// </summary>
        /// <param name="categoryId">Category ID (Optional)</param>
        /// <param name="keyword">Keyword</param>
        /// <param name="pageSize">Total record in page</param>
        /// <param name="page">Current page</param>
        /// <returns>List record</returns>
        [HttpGet]
        public IActionResult GetAllPaging(int?categoryId, string keyword, int pageSize, int page)
        {
            var model = ServiceRegistration.ProductService.GetAllPaging(categoryId, keyword, pageSize, page);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var model = ServiceRegistration.ProductCategoryService.GetAll();
            return new OkObjectResult(model);
        }
        #endregion
    }
}