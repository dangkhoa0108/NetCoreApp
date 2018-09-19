using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using CoreApp.Data.EF.Registration;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FunctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<List<FunctionViewModel>> GetAll()
        {
            return _unitOfWork.FunctionRepository.FindAll().ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public Task<List<FunctionViewModel>> GetByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
