using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using Blog.Services.ApiModels.Users;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Azure;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserApiController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserApiController(IUserService userService, ILogger<UserApiController> logger, IMapper mapper, UserManager<User> userManager)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Авторизация Аккаунта
        /// </summary>
        [HttpPost]  
        [Route("Login")]
        public async Task<IActionResult> Login(LoginApiModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("Запрос не корректен");
            
            var result = await _userService.Login(new LoginViewModel {Email=model.Email,Password=model.Password });

            if (!result.Succeeded) 
            {
                _logger.LogWarning($"Ошибка аутентификации {model.Email}");
                throw new AuthenticationException("НЕВЕРНЫЙ ЛОГИН ИЛИ ПАРОЛЬ"); 
            }
                

            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            if (roles.Contains("Admin"))
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, roles.First()));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            _logger.LogInformation($"Пользователь {user.Name} {user.LastName} успешно авторизовался для работы с API");
            return StatusCode(200);
        }

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userResponse = _mapper.Map<List<UserApiModel>>(users);
            return StatusCode(200, userResponse);
        }
        /// <summary>
        /// Получение пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var response = _mapper.Map<UserApiModel>(user);
            return StatusCode(200, response);
        }
        /// <summary>
        /// Удаление Пользователя по ID
        /// </summary>
        /// <param name="id">ID Пользователя</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id == 0)
            {
                _logger.LogWarning($"Ошибка удаление пользователя с id {id}");
                return StatusCode(400);
            }
               
            var user = await _userService.GetUserByIdAsync(id);
            if (user!=null)
            {
                await _userService.DeleteUserByIdAsync(id);
                _logger.LogInformation($"Пользователь с id {id} успешно удален");
                return StatusCode(200);
            }
            _logger.LogError($"Ошибка при удалении пользователя, пользователь с id {id} не найден");
            return StatusCode(400);   
        }
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="model">Модель Api созданя пользователя</param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreateApiModel model)
        {
            var cmodel = _mapper.Map<UserCreateViewModel>(model);
            var result = await _userService.CreateAsync(cmodel);
            if (result.Succeeded) 
            {
                _logger.LogInformation($"Пользователь {model.Name} {model.LastName} создан");
                return StatusCode(200, $"Пользователь создан");
            }
            _logger.LogError($"Произошла ошибка при создании пользователя");
            return StatusCode(400,$"произошла ошибка");
        }
        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPatch]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(UserEditApiModel model)
        {
            var emodel = _mapper.Map<UserEditViewModel>(model);
            var result = await _userService.EditAsync(emodel);
            if (result.Succeeded) { return StatusCode(200, $"пользователь обновлен"); }
            return StatusCode(400, $"произошла ошибка");
        }
    }
}
