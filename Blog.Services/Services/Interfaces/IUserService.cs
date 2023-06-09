using Blog.DAL.Models;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(UserRegisterViewModel model);
        Task<IdentityResult> CreateAsync(UserCreateViewModel model);
        Task<SignInResult> Login(LoginViewModel model);
        Task<IdentityResult> EditAsync(UserEditViewModel model);
        Task<IdentityResult> EditAdminAsync(UserEditAdminModeViewModel model, int id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task DeleteUserByIdAsync(int id);
        Task Logout();
        Task<UserEditAdminModeViewModel> GetEditUserAdminMode(int id);
        Task<IdentityResult> ChangePassword(ChangePasswordViewModel model);
    }
}
