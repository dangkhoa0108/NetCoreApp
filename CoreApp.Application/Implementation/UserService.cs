using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using CoreApp.Data.Entities;
using CoreApp.Utilities.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                DateCreated = DateTime.Now,
                PhoneNumber = appUserViewModel.PhoneNumber,
                Status = appUserViewModel.Status
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

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            //Convent from appUser model to AppUserViewModel
            return await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
        }

        public PageResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x =>
                    x.FullName.Contains(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword));
            var totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.Select(x => new AppUserViewModel
            {
                UserName = x.UserName,
                Email = x.Email,
                Avatar = x.Avatar,
                BirthDay = x.Birthday.ToString(),
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated
            }).ToList();
            var paginationSet= new PageResult<AppUserViewModel>
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public async Task<AppUserViewModel> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = Mapper.Map<AppUser, AppUserViewModel>(user);
            userViewModel.Roles = roles.ToList();
            return userViewModel;
        }

        public async Task UpdateAsync(AppUserViewModel appUserViewModel)
        {
            var user = await _userManager.FindByIdAsync(appUserViewModel.Id.ToString());
            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result =
                await _userManager.AddToRolesAsync(user, appUserViewModel.Roles.Except(currentRoles).ToArray());
            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(appUserViewModel.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);
                 
                //Update user Detail
                user.FullName = appUserViewModel.FullName;
                user.Status = appUserViewModel.Status;
                user.Email = appUserViewModel.Email;
                user.PhoneNumber = appUserViewModel.PhoneNumber;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
