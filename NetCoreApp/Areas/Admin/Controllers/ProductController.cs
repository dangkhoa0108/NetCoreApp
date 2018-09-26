using System.Collections.Generic;
using System.Linq;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [HttpGet]
        public IActionResult GetProductById(int id)
        {
            var model = ServiceRegistration.ProductService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(e => e.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            productViewModel.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
            if (productViewModel.Id==0)
            {
                ServiceRegistration.ProductService.Add(productViewModel);
            }
            else
            {
                ServiceRegistration.ProductService.Update(productViewModel);
            }
            return new OkObjectResult(productViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            ServiceRegistration.ProductService.Delete(id);
            return new OkObjectResult(id);
        }
        #endregion
    }
}