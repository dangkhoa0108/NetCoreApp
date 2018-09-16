using AutoMapper;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Application.ViewModels.System;
using CoreApp.Data.Entities;

namespace CoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile:Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
