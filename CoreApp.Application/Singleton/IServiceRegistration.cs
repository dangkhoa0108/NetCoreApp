using CoreApp.Application.Interfaces;

namespace CoreApp.Application.Singleton
{
    public interface IServiceRegistration
    {
        //Register only one (singleton)
        IFunctionService FunctionService { get; }
        IProductCategoryService ProductCategoryService { get; }
        IProductService ProductService { get; }
    }
}
