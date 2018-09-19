using System;
using System.IO;
using CoreApp.Data.EF.Repositories;
using CoreApp.Data.IRepositories;
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

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public UnitOfWork()
        {
            if (_context == null)
            {
                
            }
        }
        public IProductRepository ProductRepository =>
            _productRepository ?? (_productRepository = new ProductRepository(_context));

        public IProductCategoryRepository ProductCategoryRepository => 
            _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryRepository(_context));

        public IFunctionRepository FunctionRepository =>
            _functionRepository ?? (_functionRepository = new FunctionRepository(_context));
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
