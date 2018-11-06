using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.EF.Registration;
using CoreApp.Data.Entities;
using CoreApp.Data.Enums;
using CoreApp.Utilities.Constants;
using CoreApp.Utilities.DTOs;
using CoreApp.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CoreApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ProductViewModel> GetAll()
        {
            return _unitOfWork.ProductRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }

        public PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int pageSize, int page)
        {
            // Execute Query to Get Data
            //Show All have status active
            //var query = _unitOfWork.ProductRepository.FindAll(x => x.Status.Equals(Status.Active));

            //Show All
            var query = _unitOfWork.ProductRepository.FindAll();
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

        public ProductViewModel GetById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_unitOfWork.ProductRepository.FindById(id));
        }

        public ProductViewModel Add(ProductViewModel productViewModel)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                //Add tag
                string[] tags = productViewModel.Tags.Split(",");
                foreach (var tag in tags)
                {
                    var tagId = TextHelper.ToUnsignString(tag);
                    if (!_unitOfWork.TagRepository.FindAll(x => x.Id.Equals(tagId)).Any())
                    {
                        var tagItem = new Tag
                        {
                            Id = tagId,
                            Name = tag,
                            Type = CommonConstants.ProductTag
                        };
                        _unitOfWork.TagRepository.Add(tagItem);
                    }

                    var productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }
            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            product.DateCreated=DateTime.Now;
            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Commit();
            return productViewModel;
        }

        public void Update(ProductViewModel productViewModel)
        {
            var productTags= new List<ProductTag>();
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                string[] tags = productViewModel.Tags.Split(",");
                foreach (var tag in tags)
                {
                    var tagId = TextHelper.ToUnsignString(tag);
                    if (!_unitOfWork.TagRepository.FindAll(x => x.Id.Equals(tagId)).Any())
                    {
                        var tagItem = new Tag
                        {
                            Id = tagId,
                            Name = tag,
                            Type = CommonConstants.ProductTag
                        };
                        _unitOfWork.TagRepository.Add(tagItem);
                    }
                    //Remove all tag for product
                    _unitOfWork.ProductTagRepository.RemoveMultiple(_unitOfWork.ProductTagRepository.FindAll(x=>x.Id.Equals(productViewModel.Id)).ToList());
                    var productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }
            var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }

            //No tracking Id
            var temp = _unitOfWork.ProductRepository.FindAll(x => x.Id == productViewModel.Id).AsNoTracking().First();
            product.DateModified=DateTime.Now;
            if (temp != null) product.DateCreated = temp.DateCreated;
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _unitOfWork.ProductRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package= new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[1];
                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    var product = new Product();
                    product.CategoryId = categoryId;
                    product.Name = worksheet.Cells[i, 1].Value.ToString();
                    product.Description = worksheet.Cells[i, 2].Value.ToString();
                    decimal.TryParse(worksheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;
                    decimal.TryParse(worksheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(worksheet.Cells[i, 5].Value.ToString(), out var promotionPrice);
                    product.PromotionPrice = promotionPrice;
                    product.Content = worksheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = worksheet.Cells[i, 7].Value.ToString();
                    product.SeoDescription = worksheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(worksheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;
                    bool.TryParse(worksheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;
                    product.Status = Status.Active;
                    _unitOfWork.ProductRepository.Add(product);
                    _unitOfWork.Commit();
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
