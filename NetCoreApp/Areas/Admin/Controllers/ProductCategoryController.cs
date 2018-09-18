using System.Collections.Generic;
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
        #endregion
    }
}