using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApp.Application.ViewModels.System;
using CoreApp.Utilities.DTOs;

namespace CoreApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel appUserViewModel);
        Task DeleteAsync(string id);
        Task<List<AppUserViewModel>> GetAllAsync();
        PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
        Task<AppUserViewModel> GetByIdAsync(string id);
        Task UpdateAsync(AppUserViewModel appUserViewModel);
    }
}
