using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Comments;
using Blog.Services.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICommentService _commentService;

        public CommentController(UserManager<User> usermanager, ICommentService commentService)
        {
            _userManager = usermanager;
            _commentService = commentService;
        }

        [HttpGet]
        [Authorize]
        [Route("Comment/Add")]
        public IActionResult AddComment(int postId)
        {
            var model = new CommentAddViewModel { PostId = postId };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("Comment/Add")]
        public async Task<IActionResult> AddComment(CommentAddViewModel model, int postid)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.UserId = user.Id;
            model.PostId = postid;
            await _commentService.AddCommentAsync(model);
            return RedirectToAction("ViewPost", "Post", new {@id=postid });
        }
        [HttpGet]
        [Authorize]
        [Route("Comment/delete{id}/{postid}")]
        public async Task<IActionResult> deleteComment(int id, int postid)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("ViewPost", "Post", new { @id=postid});
        }
        [HttpGet]
        [Authorize]
        [Route(("Comment/Edit"))]
        public async Task<IActionResult> EditComment(int id)
        {
            var model =await _commentService.EditCommentAsync(id);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("Comment/Edit")]
        public async Task<IActionResult> EditComment(CommentEditViewModel model)
        {
            await _commentService.EditCommentAsync(model);
            return RedirectToAction("ViewPost", "Post", new { @id = model.PostId });
        }
    }
}
