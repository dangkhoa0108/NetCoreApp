using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApp.Application.ViewModels.System;
using CoreApp.Utilities.DTOs;

namespace CoreApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AppRoleViewModel appRoleViewModel);
        Task DeleteAsync(Guid id);
        Task<List<AppRoleViewModel>> GetAllAsync();
        PageResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
        Task<AppRoleViewModel> GetById(Guid id);
        Task UpdateAsync(AppRoleViewModel appRoleViewModel);
        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);
        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);
        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
