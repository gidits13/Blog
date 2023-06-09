using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
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
        [HttpGet]
        [Authorize]
        [Route("Role/Delete")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return RedirectToAction("GetRoles","Role");
        }
        [HttpGet]
        [Authorize]
        [Route("Role/Add")]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Route("Role/Add")]
        public async Task<IActionResult>AddRoleAsync(RoleAddViewModel model)
        {
            await _roleService.AddRoleAsync(model);
            return RedirectToAction("GetRoles", "Role");
        }
        [HttpGet]
        [Authorize]
        [Route("Role/Edit")]
        public async Task<IActionResult> EditRole(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            var model = new RoleEditViewModel() {Id=role.Id, Name=role.Name};
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("Role/Edit")]
        public async Task<IActionResult>EditRole(RoleEditViewModel model)
        {
            await _roleService.EditRoleAsync(model);
            return RedirectToAction("GetRoles", "Role");
        }
    }
}
