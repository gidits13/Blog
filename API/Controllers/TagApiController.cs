using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.ApiModels.Tags;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagApiController> _logger;
        private readonly ITagService _tagService;

        public TagApiController(IMapper mapper, ILogger<TagApiController> logger, ITagService tagService)
        {
            _mapper = mapper;
            _logger = logger;
            _tagService = tagService;
        }
        /// <summary>
        /// Получение списка всех тэгов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("GetAllTags")]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagService.GetTags();
            var response = _mapper.Map<List<TagApiModel>>(tags).Select(x => new { x.Id,x.Name});
            return StatusCode(200, response);
        }
        /// <summary>
        /// Получение тэга по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("GetTag")]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag =await _tagService.GetTagById(id);
            if(tag!=null)
            {
                var response = new { tag.Id, tag.Name };
                return StatusCode(200, response);
            }
            _logger.LogError($"Тэга по заданному ID {id} не существует");
            return StatusCode(400, $"Тэга по заданному ID {id} не существует");
        }
        /// <summary>
        /// Удаление тэга по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles ="Admin")]
        [Route("DeleteTag")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag=await _tagService.GetTagById(id);
            if (tag != null)
            {
                await _tagService.DeleteTag(id);
                return StatusCode(200, $"Тэг с ID {id} удален");

            }
            _logger.LogError($"Ошибка удаления тэга, тэга с ID {id} не уществует");
            return StatusCode(400, $"Ошибка удаления тэга, тэга с ID {id} не уществует");
        }
        /// <summary>
        /// Создание нового тэга
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [Route("AddTag")]
        public async Task<IActionResult> AddTag(TagAddApiModel model)
        {
            if(ModelState.IsValid)
            {
                var tag = new TagAddViewModel {Name = model.Name};
                await _tagService.AddTag(tag);
                return StatusCode(200, $"Тэг {tag.Name} Создан");
            }
            return StatusCode(400);
        }
        /// <summary>
        /// Изменение тэга
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles ="Admin")]
        [Route("EditTag")]
        public async Task<IActionResult> EditTag(TagEditApiModel model)
        {
            if (ModelState.IsValid) 
            {
                var tag = await _tagService.GetTagById(model.Id);
                if (tag != null) 
                {
                    var etag = new TagEditViewModel { Id = model.Id, Name = model.Name };
                    await _tagService.EditTag(etag);
                    _logger.LogInformation($"Тэг {tag.Name} изменен");
                    return StatusCode(200, $"Тэг {tag.Name} изменен");
                }
                _logger.LogInformation($"Ошибка изменения тэга {model.Name},тэг не существует");
                return StatusCode(400, $"Ошибка изменения тэга {model.Name},тэг не существует");
            }
            return StatusCode(400, $"Ошибка изменения тэга {model.Name},тэг не существует");
        }
    }
}
