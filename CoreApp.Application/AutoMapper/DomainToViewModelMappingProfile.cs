using AutoMapper;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.Entities;

namespace CoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile:Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductViewModel>();
        }
    }
}
