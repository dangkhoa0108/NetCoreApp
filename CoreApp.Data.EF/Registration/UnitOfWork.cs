using System;
using System.IO;
using CoreApp.Data.EF.Repositories;
using CoreApp.Data.IRepositories;
using CoreApp.Utilities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreApp.Data.EF.Registration
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository _productRepository;
        private IFunctionRepository _functionRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private IPermissionRepository _permissionRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public UnitOfWork()
        {
            if (_context == null)
            {
                var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(CommonConstants.DefaultAppConfig).Build();
                _context= new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(configuration.GetConnectionString(CommonConstants.DefaultConnectionString)).Options);
            }
        }
        public IProductRepository ProductRepository =>
            _productRepository ?? (_productRepository = new ProductRepository(_context));

        public IProductCategoryRepository ProductCategoryRepository => 
            _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryRepository(_context));

        public IFunctionRepository FunctionRepository =>
            _functionRepository ?? (_functionRepository = new FunctionRepository(_context));

        public ITagRepository TagRepository => _tagRepository ?? (_tagRepository = new TagRepository(_context));

        public IProductTagRepository ProductTagRepository =>
            _productTagRepository ?? (_productTagRepository = new ProductTagRepository(_context));

        public IPermissionRepository PermissionRepository =>
            _permissionRepository ?? (_permissionRepository = new PermissionRepository(_context));
        
        
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
