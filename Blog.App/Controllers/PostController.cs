using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Posts;
using Blog.Services.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, UserManager<User> userManager, IMapper mapper, ITagService tagService, ILogger<PostController> logger)
        {
            _postService = postService;
            _userManager = userManager;
            _mapper = mapper;
            _tagService = tagService;
            _logger = logger;
        }
        [HttpGet]
        [Route("Post/Add")]
        [Authorize]
        public async Task<IActionResult> AddPost()
        {
            PostAddViewModel model = new PostAddViewModel();
            var tags = await _tagService.GetTags();
            model.Tags = tags.Select(x => new TagViewModel() { Id = x.Id, Name = x.Name }).ToList();
            return View(model);
        }
        [HttpPost]
        [Route("Post/Add")]
        [Authorize]
        public async Task<IActionResult>AddPost(PostAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.UserId = user.Id;
                await _postService.AddPostAsync(model);
                _logger.LogInformation($"Сатья {model.Id} успешно добавлена пользователем {model.UserId}");
                return RedirectToAction("GetPosts","Post");
            }
            _logger.LogError($"произошла ошибка при добавлении статьи {model.Id} пользователем {model.UserId}");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("Posts")]
        public async Task<IActionResult> GetPosts()
        {
            var model = await _postService.GetAllPostsAsync(); 
            return View(model);
        }
        [HttpGet]
        [Authorize]
        [Route("Posts/View")]
        public async Task<IActionResult> ViewPost(int id)
        {
            var model =await _postService.GetPostByIdAsync(id);
            return View(model);
        }
        [HttpGet]
        [Authorize]
        [Route("Post/Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var model = await _postService.EditPostAsync(id);
            return View(model); 
        }
        [HttpPost]
        [Authorize]
        [Route("Post/Edit")]
        public async Task<IActionResult> EditPost(PostEditViewModel model, int id)
        {
            if(ModelState.IsValid)
            {
                await _postService.EditPostAsync(model, id);
                _logger.LogInformation($"Статья {model.Id} успешно изменена пользователем {User.Identity.Name}");
                return RedirectToAction("GetPosts", "Post");
            }
            _logger.LogError($"Произошла ошибка при редактировании статьи {id} пользователем {User.Identity.Name}");
            return View(model);
        }
        [HttpGet]
        [Authorize]
        [Route("Post/Delete/")]
        public async Task<IActionResult> DeletePost(int id) 
        {
            await _postService.DeletePostAsync(id);
            _logger.LogInformation($"Статья {id} удалена пользвоателем {User.Identity.Name}");
            return RedirectToAction("GetPosts", "Post");
        }
        
    }
}
