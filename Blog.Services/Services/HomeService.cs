using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class HomeService : IHomeService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public HomeService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task SetUp()
        {
            var admin = new UserRegisterViewModel { Name = "admin", LastName = "admin", Email = "admin@admin.ru", Password = "1qaz2wsx", Password2 = "1qaz2wsx" };
            var moderator = new UserRegisterViewModel { Name = "moderator", LastName = "moderator", Email = "moderator@moderator.ru", Password = "1qaz2wsx", Password2 = "1qaz2wsx" };
            var userr = new UserRegisterViewModel { Name = "user", LastName = "user", Email = "user@user.ru", Password = "1qaz2wsx", Password2 = "1qaz2wsx" };
            var user=_mapper.Map<User>(admin);

            var adminRole = new Role { Name = "Admin", NormalizedName = "ADMIN" };
            var moderRole = new Role { Name = "Moderator", NormalizedName = "MODERATOR" };
            var userRole = new Role { Name = "User", NormalizedName = "USER" };

            var roles = new List<Role>() { adminRole};
            await _userManager.CreateAsync(user, admin.Password);
            await _roleManager.CreateAsync(adminRole);
            await _userManager.AddToRoleAsync(user, adminRole.Name);
			
            user.Roles = roles;
            await _userManager.UpdateAsync(user);

            roles = new List<Role>() { moderRole };
            user=_mapper.Map<User>(moderator);
            await _userManager.CreateAsync(user, moderator.Password);
            await _roleManager.CreateAsync(moderRole);
            await _userManager.AddToRoleAsync(user, moderRole.Name);
            user.Roles=roles;
            await _userManager.UpdateAsync(user);

            roles = new List<Role>() { userRole };
            user = _mapper.Map<User>(userr);
            await _userManager.CreateAsync(user, userr.Password);
            await _roleManager.CreateAsync(userRole);
            await _userManager.AddToRoleAsync(user, userRole.Name);
            user.Roles = roles;
            await _userManager.UpdateAsync(user);
        }
    }
}
