using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.Services;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Roles;
using Blog.Services.ViewModels.Tags;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Blog.App.Controllers
{
    public class UserController : Controller
    {
        //private readonly SignInManager<User> _signInManager;
        //private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserController> _logger;

        //public UserController(SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService)
        public UserController(IUserService userService, IMapper mapper, IRoleService roleService, ILogger<UserController> logger)
        {
            // _signInManager = signInManager;
            // _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
            _logger = logger;
        }
        [HttpGet]
        [Route("Register")]
        public IActionResult Register() { return View(); }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Regsiter(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Зарегестрирован новый пользователь { model.Name} - { model.LastName}");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        [Authorize]
		[Route("User/Create")]
		public IActionResult CreateUser() 
        {
            var model = new UserCreateViewModel();
            var roles = _roleService.GetRoles();
            model.Roles = roles.Select(r => new RoleViewModel() { Id = r.Id, Name = r.Name, }).ToList();
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("User/Create")]
        public async Task<IActionResult> CreateUser(UserCreateViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var result = await _userService.CreateAsync(model);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Зарегестрирован новый пользователь {model.Name} - {model.LastName}");
                return RedirectToAction("GetUsers", "User");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

       
        [HttpGet]
        [Route("Login")]
        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
				
				var result = await _userService.Login(model);
				if (result.Succeeded)
                {
                    _logger.LogInformation($"Успешный вход с систему {model.Email}");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "error");
                    _logger.LogWarning($"Ошибка аутентификации {model.Email}");
                }
            }
            return View(model);
        }
        [HttpGet]
        [Route("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            _logger.LogInformation($"Logout successfully signed out. {User.Identity.Name}");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Users")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetUsers()
        {
            UsersViewModel model = new UsersViewModel();
            var users = await _userService.GetAllUsersAsync();
            model.Users = users;
            return View(model);
        }

        [HttpGet]
        [Route("Users/Delete")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult>DeleteUser(int id)
        {
            await _userService.DeleteUserByIdAsync(id);
            _logger.LogInformation($"Delete user {id}");
            return RedirectToAction("GetUsers", "User");
        }
        [HttpGet]
        [Route("User")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        [HttpGet]
        [Route("User/Edit")]
        [Authorize]
        public async Task<IActionResult> EditUser(int id)
        {
            var user=await _userService.GetUserByIdAsync(id);
            var model = _mapper.Map<UserEditViewModel>(user);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("User/Edit")]
        public async Task<IActionResult>EditUser(UserEditViewModel model)
        {
            var result = await _userService.EditAsync(model);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Данны пользователя {model.Email} успешно изменены");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", result.Errors.FirstOrDefault().Description);
                _logger.LogError($"Произошла ошибка при редактирвоании данных пользователя {model.Email}");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("User/EditAdmin")]
        public async Task<IActionResult> EditUserAdminMode(int id)
        {
            var model = await _userService.GetEditUserAdminMode(id);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("User/EditAdmin")]
        public async Task<IActionResult> EditUserAdminMode(UserEditAdminModeViewModel model, int id)
        {
            var result =await _userService.EditAdminAsync(model, id);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Данны пользователя {model.Email} успешно изменены");
                return RedirectToAction("GetUsers", "User");
            }
            else {
                _logger.LogError($"Произошла ошибка при редактирвоании данных пользователя {model.Email}");
                return View(model);
            } 
		}
        [HttpGet]
        [Authorize]
        [Route("User/ChangePassword")]
        public async Task<IActionResult> ChangePassword(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var model = new ChangePasswordViewModel() { UserId = user.Id }; 
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("User/ChangePassword")]
        public async Task<IActionResult>ChangePassword(ChangePasswordViewModel model)
        {
            var result =await _userService.ChangePassword(model);
            if (result.Succeeded)
            {
                _logger.LogInformation($"пароль пользователя {model.UserId} успешно изменен");
                return RedirectToAction("GetUser", "User", new { @id = model.UserId });
            }
            else
            {
                _logger.LogError($"Произошла ошибка при смене пароля пользователя {model.UserId}");
                return View(model);
            }
                
        }
    }
}
