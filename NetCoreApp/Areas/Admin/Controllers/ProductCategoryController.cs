using System.Collections.Generic;
using System.Linq;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        /// <summary>
        /// Update Parent ID
        /// </summary>
        /// <param name="sourceId">Source ID </param>
        /// <param name="targetId">Target ID</param>
        /// <param name="items">Items</param>
        /// <returns>Ok result if Update success</returns>
        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }
            _productCategoryService.UpdateParentId(sourceId, targetId, items);
            _productCategoryService.Save();
            return new OkResult();
        }

        /// <summary>
        /// Update Order Id 
        /// </summary>
        /// <param name="sourceId">Source ID</param>
        /// <param name="targetId">Target ID</param>
        /// <returns>Ok result if update success</returns>
        [HttpPost]
        public IActionResult UpdateOrderId(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }
            _productCategoryService.ReOrder(sourceId, targetId);
            _productCategoryService.Save();
            return new OkResult();
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return new ObjectResult(_productCategoryService.GetById(id));
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(e => e.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            productCategoryViewModel.SeoAlias = TextHelper.ToUnsignString(productCategoryViewModel.Name);
            if (productCategoryViewModel.Id == 0)
            {
                _productCategoryService.Add(productCategoryViewModel);
            }
            else
            {
                _productCategoryService.Update(productCategoryViewModel);
            }
            _productCategoryService.Save();
            return new OkObjectResult(productCategoryViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            _productCategoryService.Delete(id);
            _productCategoryService.Save();
            return new OkResult();
        }
        #endregion
    }
}