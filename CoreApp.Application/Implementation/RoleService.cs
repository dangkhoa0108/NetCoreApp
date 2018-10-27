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
using CoreApp.Utilities.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task<bool> AddAsync(AppRoleViewModel appRoleViewModel)
        {
            var role = new AppRole
            {
                Name = appRoleViewModel.Name,
                Description = appRoleViewModel.Description
            };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _roleManager.DeleteAsync(await _roleManager.FindByIdAsync(id.ToString()));
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToListAsync();
        }

        public PageResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Description.Contains(keyword) || x.Name.Contains(keyword));
            var totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<AppRoleViewModel>().ToList();
            var paginationSet = new PageResult<AppRoleViewModel>
            {
                RowCount = totalRow,
                CurrentPage = page,
                PageSize = pageSize,
                Results = data
            };
            return paginationSet;
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            return Mapper.Map<AppRole, AppRoleViewModel>(await _roleManager.FindByIdAsync(id.ToString()));
        }

        public async Task UpdateAsync(AppRoleViewModel appRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(appRoleViewModel.Id.ToString());
            if (role != null)
            {
                role.Description = appRoleViewModel.Description;
                role.Name = appRoleViewModel.Name;
                await _roleManager.UpdateAsync(role);
            }
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _unitOfWork.FunctionRepository.FindAll();
            var permissions = _unitOfWork.PermissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = p != null && p.CanCreate,
                            CanDelete = p != null && p.CanDelete,
                            CanRead = p != null && p.CanRead,
                            CanUpdate = p != null && p.CanUpdate
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissions, Guid roleId)
        {
            var per = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissions);
            var oldPermission = _unitOfWork.PermissionRepository.FindAll().Where(x => x.RoleId.Equals(roleId)).ToList();
            if (oldPermission.Any())
            {
                _unitOfWork.PermissionRepository.RemoveMultiple(oldPermission);
            }
            foreach (var permission in per)
            {
                _unitOfWork.PermissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _unitOfWork.FunctionRepository.FindAll();
            var permissions = _unitOfWork.PermissionRepository.FindAll();
            var query = from f in functions
                join p in permissions on f.Id equals p.FunctionId
                join r in _roleManager.Roles on p.RoleId equals r.Id
                where roles.Contains(r.Name) && f.Id == functionId
                                             && (p.CanCreate && action == "Create"
                                                 || p.CanUpdate && action == "Update"
                                                 || p.CanDelete && action == "Delete"
                                                 || p.CanRead && action == "Read")
                select p;
            return query.AnyAsync();
        }
    }
}
