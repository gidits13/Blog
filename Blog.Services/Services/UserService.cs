using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels;
using Blog.Services.ViewModels.Roles;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _sigInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly ICrutch _crutch;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IMapper mapper, ICrutch crutch)
        {
            _userManager = userManager;
            _sigInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _crutch = crutch;
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> EditAsync(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            user.Name = model.Name;
            user.LastName = model.LastName;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            var result = await _userManager.UpdateAsync(user);
            return result;

        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.Include(u=>u.Roles).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<SignInResult> Login(LoginViewModel model)
        {
            return await _sigInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            //return await _signInManager.SignInWithClaimsAsync(user, false, await _userService.GetClaimsAsync(user));
        }
        public async Task<IdentityResult> CreateAsync(UserCreateViewModel model)
        {
            var userRoles = new List<Role>();
            var roles = _roleManager.Roles.ToList();
            var user = _mapper.Map<User>(model);
            if (model.Roles != null)
            {
                var rolesCheckedIds = model.Roles.Where(r => r.isChecked == true).Select(r => r.Id).ToList();

                userRoles = roles.Where(r => rolesCheckedIds.Contains(r.Id)).ToList();
            }
            await _userManager.CreateAsync(user, model.Password);
            foreach (var role in userRoles)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            user.Roles = userRoles;
            var result = await _userManager.UpdateAsync(user);

            // user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            //user.Roles = await GetRolesFromDict(model.Roles);

            //user.Roles = userRoles;
            //result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var role = await _roleManager.FindByNameAsync("User");

            user.Roles = new List<Role> { role };

            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                await _sigInManager.SignInAsync(user, false);
                return result;
            }
            return result;
        }

        public async Task Logout()
        {
            await _sigInManager.SignOutAsync();
        }

        public async Task<UserEditAdminModeViewModel> GetEditUserAdminMode(int id)
        {
            var user = await GetUserByIdAsync(id);
            var userRoles = user.Roles.ToList();
            var roles = _roleManager.Roles.ToList();

            var rolesView = roles.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name }).ToList();
            foreach (var role in rolesView)
            {
                foreach (var uRole in user.Roles)
                {
                    if (uRole.Name == role.Name)
                    {
                        role.isChecked = true;
                        break;
                    }
                }
            }

            UserEditAdminModeViewModel model = new UserEditAdminModeViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Roles = rolesView
            };
            return model;

        }

        public async Task<IdentityResult> EditAdminAsync(UserEditAdminModeViewModel model, int id)
        {
            var userRoles = new List<Role>();
            var roles = _roleManager.Roles.ToList();
            var user = await _userManager.FindByIdAsync(id.ToString());
            // var currentRoles = _roleManager.rol(user); 
            user.Name = model.Name;
            user.LastName = model.LastName;
            user.Email = model.Email;
            if (model.Roles != null)
            {
                var rolesCheckedIds = model.Roles.Where(r => r.isChecked == true).Select(r => r.Id).ToList();

                userRoles = roles.Where(r => rolesCheckedIds.Contains(r.Id)).ToList();
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            foreach (var role in userRoles)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            _crutch.ClearUserRolesList(user.Id);
            user.Roles = userRoles;

            // user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            //user.Roles = await GetRolesFromDict(model.Roles);
            var result = await _userManager.UpdateAsync(user);
            //user.Roles = userRoles;
            //result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await GetUserByIdAsync(model.UserId);
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            return await _userManager.UpdateAsync(user);
        }
    }
}
