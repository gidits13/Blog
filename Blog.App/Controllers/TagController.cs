using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;

namespace Blog.App.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }
        /// <summary>
        /// Возвращает представление для просмотра всех тэгов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Tags")]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagService.GetTags();
            var model = new TagsViewModel { Tags = tags };
            return View(model);
        }
        /// <summary>
        /// Возвращает представление для просмотра тэга
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Tag/Add")]
        public IActionResult AddTag()
        {
            var model = new TagAddViewModel();
            return View(model);
        }
        /// <summary>
        /// Создние нового тэга
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("Tag/Add")]
        public async Task<IActionResult> AddTag(TagAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _tagService.AddTag(model);
                _logger.LogInformation($"Тэг {model.Name} успешно создан");
                return RedirectToAction("GetTags", "Tag");
            }
            _logger.LogError($"Произошла ошибка при добавленни тэга {model.Name} пользователем {User.Identity.Name}");
            return View(model);
            
        }
        /// <summary>
        /// Удаление тэга
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Tag/Delete")]
        public async Task<IActionResult> deleteTag(int id)
        {
            await _tagService.DeleteTag(id);
            _logger.LogInformation($"Тэг {id} успешно удален");
            return RedirectToAction("GetTags", "Tag");
        }
        /// <summary>
        /// Возвращаетпредставление для редактирования тэга
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Tag/Edit")]
        public async Task<IActionResult> EditTag(int id)
        {
            var model =await _tagService.EditTag(id);
            return View(model);
        }
        /// <summary>
        /// изменение тега
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Tag/Edit")]
        public async Task<IActionResult> EditTag(TagEditViewModel model)
        {
            if (ModelState.IsValid) 
            {
                await _tagService.EditTag(model);
                _logger.LogInformation($"Тэг {model.Name} успешно изменен");
                return RedirectToAction("GetTags", "Tag");
            }
            _logger.LogError($"ошибка редактирования тэга {model.Name} пользователем {User.Identity.Name}");
            return View(model);

        }
    }
}
