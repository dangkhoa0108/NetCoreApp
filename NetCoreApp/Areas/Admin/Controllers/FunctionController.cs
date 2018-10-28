using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.Application.ViewModels.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllFilter(string filter)
        {
            return new ObjectResult(ServiceRegistration.FunctionService.GetAll(filter));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await ServiceRegistration.FunctionService.GetAll(string.Empty);
            var rootFunction = model.Where(f => f.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var functionViewModel in rootFunction)
            {
                //add the parent category to the item list
                items.Add(functionViewModel);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), functionViewModel, items);
            }
            return new ObjectResult(items);
        }
        [HttpGet]
        public IActionResult GetById(string id)
        {
            return new ObjectResult(ServiceRegistration.FunctionService.GetById(id));
        }
        [HttpPost]
        public IActionResult SaveEntity(FunctionViewModel function)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (!string.IsNullOrWhiteSpace(function.Id))
            {
                ServiceRegistration.FunctionService.Update(function);
            }
            ServiceRegistration.FunctionService.Add(function);
            return new OkObjectResult(function);
        }
        [HttpPost]
        public IActionResult UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }

            ServiceRegistration.FunctionService.UpdateParentId(sourceId, targetId, items);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult ReOrder(string sourceId, string targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (sourceId == targetId)
            {
                return new BadRequestResult();
            }
            ServiceRegistration.FunctionService.ReOrder(sourceId, targetId);
            return new OkObjectResult(sourceId);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            ServiceRegistration.FunctionService.Delete(id);
            return new OkObjectResult(id);
        }

        #region Private Functions
        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions,
            FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionsEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();
            var subFunctions = functionsEntities.Where(c => c.ParentId == parent.Id);
            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }
        #endregion
    }
}