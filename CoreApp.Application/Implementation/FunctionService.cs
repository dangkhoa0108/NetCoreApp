using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using CoreApp.Data.EF.Registration;
using CoreApp.Data.Entities;
using CoreApp.Data.Enums;
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

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _unitOfWork.FunctionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));
            return query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public void Add(FunctionViewModel function)
        {
            _unitOfWork.FunctionRepository.Add(Mapper.Map<FunctionViewModel, Function>(function));
            _unitOfWork.Commit();
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return _unitOfWork.FunctionRepository.FindAll(x => x.ParentId.Equals(parentId))
                .ProjectTo<FunctionViewModel>();
        }

        public FunctionViewModel GetById(string id)
        {
            return Mapper.Map<Function, FunctionViewModel>(_unitOfWork.FunctionRepository.FindById(id));
        }

        public void Update(FunctionViewModel function)
        {
            var functionItem = _unitOfWork.FunctionRepository.FindById(function.Id);
            var map = Mapper.Map<FunctionViewModel, Function>(function);
            functionItem = map;
            _unitOfWork.FunctionRepository.Update(functionItem);
            _unitOfWork.Commit();
        }

        public void Delete(string id)
        {
            _unitOfWork.FunctionRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public bool CheckExistedId(string id)
        {
            return _unitOfWork.FunctionRepository.FindById(id) != null;
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            var function = _unitOfWork.FunctionRepository.FindById(sourceId);
            function.ParentId = targetId;
            _unitOfWork.FunctionRepository.Update(function);

            var sibling = _unitOfWork.FunctionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _unitOfWork.FunctionRepository.Update(child);
            }
            _unitOfWork.Commit();
        }

        public void ReOrder(string source, string target)
        {
            var sourceItem = _unitOfWork.FunctionRepository.FindById(source);
            var targetItem = _unitOfWork.FunctionRepository.FindById(target);
            var temp = sourceItem.SortOrder;
            sourceItem.SortOrder = targetItem.SortOrder;
            targetItem.SortOrder = temp;
            _unitOfWork.FunctionRepository.Update(sourceItem);
            _unitOfWork.FunctionRepository.Update(targetItem);
            _unitOfWork.Commit();
        }
    }
}
