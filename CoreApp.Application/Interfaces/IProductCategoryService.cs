using System.Collections.Generic;
using CoreApp.Application.ViewModels.Product;

namespace CoreApp.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryViewModel);
        void Update(ProductCategoryViewModel productCategoryViewModel);
        void Delete(int id);
        List<ProductCategoryViewModel> GetAll();
        List<ProductCategoryViewModel> GetAll(string keyword);
        List<ProductCategoryViewModel> GetAllByParentId(int parentId);
        ProductCategoryViewModel GetById(int id);
        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        void ReOrder(int sourceId, int targetId);
        List<ProductCategoryViewModel> GetHomeCategories(int top);
        void Save();
    }
}
