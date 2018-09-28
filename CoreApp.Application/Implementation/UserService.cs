using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using CoreApp.Data.Entities;
using CoreApp.Utilities.DTOs;
using Microsoft.AspNetCore.Identity;

namespace CoreApp.Application.Implementation
{
    public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> AddAsync(AppUserViewModel appUserViewModel)
        {
            var user =new AppUser
            {
                UserName = appUserViewModel.UserName,
                Avatar = appUserViewModel.Avatar,
                Email = appUserViewModel.Email,
                FullName = appUserViewModel.FullName,
                DateCreated = appUserViewModel.DateCreated,
                PhoneNumber = appUserViewModel.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, appUserViewModel.Password);
            if (!result.Succeeded || appUserViewModel.Roles.Count <= 0) return false;
            var appUser = await _userManager.FindByNameAsync(user.UserName);
            if (appUser != null)
            {
                await _userManager.AddToRolesAsync(appUser, appUserViewModel.Roles);
            }
            return true;
        }

        public Task DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<AppUserViewModel>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppUserViewModel> GetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(AppUserViewModel appUserViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
