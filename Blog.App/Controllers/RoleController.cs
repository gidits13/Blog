using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        /// <summary>
        /// Возвращает представление для отображения всех ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleService.GetRoles();
            var model = new RolesViewModel();
            model.Roles = roles;
            return View(model);
        }
        /// <summary>
        /// удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Role/Delete")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            _logger.LogInformation($"Роль  {id} удалена пользователем {User.Identity.Name}");
            return RedirectToAction("GetRoles","Role");
        }
        /// <summary>
        /// Возвращает представление для создания новой роли
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Role/Add")]
        public IActionResult AddRole()
        {
            return View();
        }
        /// <summary>
        /// Создание новой роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Role/Add")]
        public async Task<IActionResult>AddRoleAsync(RoleAddViewModel model)
        {
            await _roleService.AddRoleAsync(model);
            _logger.LogInformation($"Роль {model.Name} успешно создана");
            return RedirectToAction("GetRoles", "Role");
        }
        /// <summary>
        /// возвращает представление для редактирования роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Role/Edit")]
        public async Task<IActionResult> EditRole(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            var model = new RoleEditViewModel() {Id=role.Id, Name=role.Name};
            return View(model);
        }
        /// <summary>
        /// Внесение изменений в роль
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Role/Edit")]
        public async Task<IActionResult>EditRole(RoleEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                await _roleService.EditRoleAsync(model);
                _logger.LogInformation($"Роль {model.Name} успешно изменена");
                return RedirectToAction("GetRoles", "Role");
            }
            _logger.LogError($"Произошла ошибка при редактирвоании роли {model.Id} пользователем {User.Identity.Name}");
            return View(model);
            
        }
    }
}
