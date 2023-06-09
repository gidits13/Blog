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

        public PostController(IPostService postService,UserManager<User> userManager, IMapper mapper, ITagService tagService)
        {
            _postService = postService;
            _userManager = userManager;
            _mapper = mapper;
            _tagService = tagService;
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
                return RedirectToAction("GetPosts","Post");
            }
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
            await _postService.EditPostAsync(model, id);
            return RedirectToAction("GetPosts", "Post");
        }
        [HttpGet]
        [Authorize]
        [Route("Post/Delete/")]
        public async Task<IActionResult> DeletePost(int id) 
        {
            await _postService.DeletePostAsync(id);
            return RedirectToAction("GetPosts", "Post");
        }
        
    }
}
