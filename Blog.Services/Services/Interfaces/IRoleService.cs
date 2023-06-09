using Blog.DAL.Models;
using Blog.Services.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Interfaces
{
    public interface IRoleService
    {
        List<Role> GetRoles();
        Task DeleteRoleAsync(int id);
        Task AddRoleAsync(RoleAddViewModel model);
        Task<Role> GetRoleAsync(int id);
        Task EditRoleAsync(RoleEditViewModel model);
    }
}
