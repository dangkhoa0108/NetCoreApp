using System;
using System.Collections.Generic;
using CoreApp.Application.ViewModels.Product;

namespace CoreApp.Application.Interfaces
{
    public interface IProductService:IDisposable
    {
        List<ProductViewModel> GetAll();
    }
}
