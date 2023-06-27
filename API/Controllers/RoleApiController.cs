using AutoMapper;
using Blog.Services.ApiModels.Roles;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleApiController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleApiController> _logger;
        private readonly IMapper _mapper;

        public RoleApiController(IRoleService roleService, ILogger<RoleApiController> logger, IMapper mapper)
        {
            _roleService = roleService;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Получение списка всех ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetRoles();
            var response = _mapper.Map<List<RoleApiModel>>(roles);
            return StatusCode(200, response);
        }
        /// <summary>
        /// Получение роли по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [Route("GetRole")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = _roleService.GetRoleAsync(id);
            if (role != null) 
            {
                var response = _mapper.Map<RoleApiModel>(role);
                return StatusCode(200, response);
            }
            return StatusCode(400, $"Роль не найдена");
        }
        /// <summary>
        /// Изменение Роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles ="Admin")]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole(RoleEditApiModel model)
        {
            if(ModelState.IsValid)
            {
                var role= await _roleService.GetRoleAsync(model.Id);
                if(role !=null)
                {
                    var erole=new RoleEditViewModel { Id = role.Id, Name=model.Name };
                    await _roleService.EditRoleAsync(erole);
                    _logger.LogInformation($"Роль {role.Name} успешно изменена, новое название {model.Name}");
                    return StatusCode(200, $"Роль {role.Name} успешно изменена, новое название {model.Name}");
                }
                _logger.LogError($"Ошибка изменение роли с ID {model.Id}, роль не существует");
                return StatusCode(400,$"Ошибка изменение роли с ID {model.Id}, роль не существует");
            }
            return StatusCode(400,$"Ошибка изменения роли");
        }
        /// <summary>
        /// Удаление Роли по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles ="Admin")]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            if(role != null)
            {
                await _roleService.DeleteRoleAsync(id);
                _logger.LogInformation($"Роль {role.Name} удалена");
                return StatusCode(200, $"Роль {role.Name} удалена");
            }
            _logger.LogError($"Ошибка удаления роли, роль с ID {id} не существует");
            return StatusCode(400, $"Ошибка удаления роли, роль с ID {id} не существует");
        }
        /// <summary>
        /// Создание новой роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(RoleAddApiModel model)
        {
            if(ModelState.IsValid)
            {
                var crole = new RoleAddViewModel {  Name = model.Name };
                await _roleService.AddRoleAsync(crole);
                _logger.LogInformation($"Роль {model.Name} создана");
                return StatusCode(200, $"Роль {model.Name} создана");
            }
            return StatusCode(400);
        }
    }
}
