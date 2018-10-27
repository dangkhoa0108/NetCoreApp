using CoreApp.Application.Implementation;
using CoreApp.Application.Interfaces;
using CoreApp.Data.EF.Registration;

namespace CoreApp.Application.Singleton
{
    public class ServiceRegistration : IServiceRegistration
    {
        private readonly IUnitOfWork _unitOfWork;
        private IFunctionService _functionService;
        private IProductCategoryService _productCategoryService;
        private IProductService _productService;

        public ServiceRegistration(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceRegistration()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IFunctionService FunctionService =>
            _functionService ?? (_functionService = new FunctionService(_unitOfWork));

        public IProductCategoryService ProductCategoryService =>
            _productCategoryService ?? (_productCategoryService = new ProductCategoryService(_unitOfWork));

        public IProductService ProductService => _productService ?? (_productService = new ProductService(_unitOfWork));
        //public IUserService UserService => _userService ?? (_userService = new UserService(_userManager));
        //public IRoleService RoleService => _roleService ?? (_roleService = new RoleService(_unitOfWork, _roleManager));
    }
}
