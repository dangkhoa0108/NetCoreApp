using System.Collections.Generic;
using System.Linq;
using CoreApp.Data.Entities;
using CoreApp.Data.IRepositories;

namespace CoreApp.Data.EF.Repositories
{
    public class ProductCategoryRepository: EFRepository<ProductCategory, int> ,IProductCategoryRepository
    {
        private readonly AppDbContext _context;
        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(x => x.SeoAlias.Equals(alias)).ToList();
        }
    }
}
