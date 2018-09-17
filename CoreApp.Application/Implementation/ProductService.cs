using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.Enums;
using CoreApp.Data.IRepositories;
using CoreApp.Utilities.DTOs;

namespace CoreApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }

        public PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int pageSize, int page)
        {
            // Execute Query to Get Data
            var query = _productRepository.FindAll(x => x.Status.Equals(Status.Active));
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            int totalRow = query.Count();
            query = query.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);

            //Convert to ViewModel
            var data = query.ProjectTo<ProductViewModel>().ToList();

            //Add data to Page Result
            var paging = new PageResult<ProductViewModel>
            {
                PageSize = pageSize,
                RowCount = totalRow,
                Results = data,
                CurrentPage = page
            };
            return paging;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
