using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

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

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');
                var folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                ServiceRegistration.ProductService.ImportExcel(filePath, categoryId);
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }
        #endregion
    }
}