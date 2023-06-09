using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task AddRoleAsync(RoleAddViewModel model)
        {
            var role = new Role() { Name=model.Name,NormalizedName=model.Name.ToUpper()};
            await _roleManager.CreateAsync(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task EditRoleAsync(RoleEditViewModel model)
        {
            var role = await GetRoleAsync(model.Id);
            role.Name = model.Name;
            await _roleManager.UpdateAsync(role);
        }

        public async Task<Role> GetRoleAsync(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public List<Role> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }

    }
}
