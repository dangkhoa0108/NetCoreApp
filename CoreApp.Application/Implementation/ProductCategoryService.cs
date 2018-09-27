using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.EF.Registration;
using CoreApp.Data.Entities;
using CoreApp.Data.Enums;

namespace CoreApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryViewModel)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel);
            _unitOfWork.ProductCategoryRepository.Add(productCategory);
            _unitOfWork.Commit();
            return productCategoryViewModel;
        }

        public void Update(ProductCategoryViewModel productCategoryViewModel)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel);
            _unitOfWork.ProductCategoryRepository.Update(productCategory);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _unitOfWork.ProductCategoryRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _unitOfWork.ProductCategoryRepository.FindAll().OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return _unitOfWork.ProductCategoryRepository.FindAll(x => x.Description.Contains(keyword) || x.Name.Contains(keyword))
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            }

            return _unitOfWork.ProductCategoryRepository.FindAll().ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _unitOfWork.ProductCategoryRepository.FindAll(x => x.Status.Equals(Status.Active) && x.ParentId.Equals(parentId))
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_unitOfWork.ProductCategoryRepository.FindById(id));
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            //Get Source Category
            var sourceCategory = _unitOfWork.ProductCategoryRepository.FindById(sourceId);
            //Assign Target Id to source Category
            sourceCategory.ParentId = targetId;
            //Update
            _unitOfWork.ProductCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var listSibling = _unitOfWork.ProductCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var productCategory in listSibling)
            {
                productCategory.SortOrder = items[productCategory.Id];
                _unitOfWork.ProductCategoryRepository.Update(productCategory);
            }
            _unitOfWork.Commit();
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _unitOfWork.ProductCategoryRepository.FindById(sourceId);
            var target = _unitOfWork.ProductCategoryRepository.FindById(targetId);
            var temp = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = temp;
            _unitOfWork.ProductCategoryRepository.Update(source);
            _unitOfWork.ProductCategoryRepository.Update(target);
            _unitOfWork.Commit();
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _unitOfWork.ProductCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Products)
                .OrderBy(x => x.HomeOrder)
                .Take(top).ProjectTo<ProductCategoryViewModel>();

            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products = _productRepository
                //    .FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }
    }
}
