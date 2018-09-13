using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApp.Application.ViewModels.System;

namespace CoreApp.Application.Interfaces
{
    public interface IFunctionService:IDisposable
    {
        Task<List<FunctionViewModel>> GetAll();
        Task<List<FunctionViewModel>> GetByPermission(Guid userId);
    }
}
