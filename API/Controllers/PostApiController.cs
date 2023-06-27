using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.ApiModels.Posts;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Posts;
using Blog.Services.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly ILogger<PostApiController> _logger;
        private readonly UserManager<User> _usermanager;

        public PostApiController(IPostService postService, IMapper mapper, ILogger<PostApiController> logger, UserManager<User> userManager)
        {
            _postService = postService;
            _mapper = mapper;
            _logger = logger;
            _usermanager= userManager;
        }
        /// <summary>
        /// Получение всех статей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            List<Post> list = new List<Post>();
            list = posts.Posts;
            var respons = _mapper.Map<List<PostApiModel>>(list);
            return StatusCode(200, respons);
        }
        /// <summary>
        /// Получение статьи по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetPost")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            var response = _mapper.Map<PostApiModel>(post);
            return StatusCode(200, response);
        }
        /// <summary>
        /// Создание статьи 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost(PostAddApiModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _usermanager.FindByEmailAsync(User.Identity.Name);

                var tags = _mapper.Map<List<TagViewModel>>(model.Tags);
                PostAddViewModel pmodel = new PostAddViewModel { UserId = user.Id, Text = model.Text, Tags = tags, Title = model.Title };
                await _postService.AddPostAsync(pmodel);
                _logger.LogInformation($"Статья создана, Title {pmodel.Title}");
                return StatusCode(200, pmodel);
            }
            return StatusCode(400, $"Ошибка создания статьи");

        }
        /// <summary>
        /// удаление статьи по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            _logger.LogInformation($"Статья с id {id} удалена");
            return StatusCode(200, $"Статья с id {id} удалена");
        }
        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles ="Admin")]
        [Route("EditPost")]
        public async Task<IActionResult> EditPost(PostEditApiModel model)
        {
            if(ModelState.IsValid)
            {
                var post = await _postService.GetPostByIdAsync(model.Id);
                if(post !=null)
                {
                    var tags = _mapper.Map<List<TagViewModel>>(model.Tags);
                    var pmodel = new PostEditViewModel { Text = model.Text, Title = model.Title, Tags = tags };
                        await _postService.EditPostAsync(pmodel, model.Id);
                    _logger.LogInformation($"Статья с id {model.Id}  изменена");
                    return StatusCode(200, $"Статья с id {model.Id}  изменена");
                }
                else
                {
                    _logger.LogError($"Попытка редактировать несуществующую статью ID {model.Id}");
                    return StatusCode(400, $"Попытка редактировать несуществующую статью ID {model.Id}");
                }

            }
            return StatusCode(400);

        }

    }
}
