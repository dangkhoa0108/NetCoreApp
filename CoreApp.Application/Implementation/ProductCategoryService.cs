using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.Entities;
using CoreApp.Data.Enums;
using CoreApp.Data.IRepositories;
using CoreApp.Infrastructure.Interfaces;

namespace CoreApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategory;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategory, IUnitOfWork unitOfWork)
        {
            _productCategory = productCategory;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryViewModel)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel);
            _productCategory.Add(productCategory);
            return productCategoryViewModel;
        }

        public void Update(ProductCategoryViewModel productCategoryViewModel)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryViewModel);
            _productCategory.Update(productCategory);
        }

        public void Delete(int id)
        {
            _productCategory.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _productCategory.FindAll().OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return _productCategory.FindAll(x => x.Description.Contains(keyword) || x.Name.Contains(keyword))
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            }

            return _productCategory.FindAll().ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategory
                .FindAll(x => x.Status.Equals(Status.Active) && x.ParentId.Equals(parentId))
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategory.FindById(id));
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            //Get Source Category
            var sourceCategory = _productCategory.FindById(sourceId);
            //Assign Target Id to source Category
            sourceCategory.ParentId = targetId;
            //Update
            _productCategory.Update(sourceCategory);

            //Get all sibling
            var listSibling = _productCategory.FindAll(x => items.ContainsKey(x.Id));
            foreach (var productCategory in listSibling)
            {
                productCategory.SortOrder = items[productCategory.Id];
                _productCategory.Update(productCategory);
            }
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategory.FindById(sourceId);
            var target = _productCategory.FindById(targetId);
            var temp = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = temp;
            _productCategory.Update(source);
            _productCategory.Update(target);
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategory
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

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
