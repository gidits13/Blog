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
        private readonly ILogger<CommentController> _logger;
        public CommentController(UserManager<User> usermanager, ICommentService commentService, ILogger<CommentController> logger)
        {
            _userManager = usermanager;
            _commentService = commentService;
            _logger = logger;
        }
        /// <summary>
        /// Метод возвращет представление для добавления комментария к статье
        /// </summary>
        /// <param name="postId">ID статьи</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Comment/Add")]
        public IActionResult AddComment(int postId)
        {
            var model = new CommentAddViewModel { PostId = postId };
            return View(model);
        }
        /// <summary>
        /// Добавление комментария к статье
        /// </summary>
        /// <param name="model"></param>
        /// <param name="postid"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Comment/Add")]
        public async Task<IActionResult> AddComment(CommentAddViewModel model, int postid)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.UserId = user.Id;
                model.PostId = postid;
                await _commentService.AddCommentAsync(model);
                _logger.LogInformation($"Комментарий к статье {model.PostId} успешно добавлено пользователем {model.UserId}");
                return RedirectToAction("ViewPost", "Post", new { @id = postid });
            }
            _logger.LogError($"Произошла ошибка при добавлении комментария к статье {postid}");
            return View(model);
        }
        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id"></param>
        /// <param name="postid"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Comment/delete{id}/{postid}")]
        public async Task<IActionResult> deleteComment(int id, int postid)
        {
            await _commentService.DeleteCommentAsync(id);
            _logger.LogInformation($"deleted {postid}");
            return RedirectToAction("ViewPost", "Post", new { @id=postid});
        }
        /// <summary>
        /// Метод возвращает предствление для редактирования комментария
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route(("Comment/Edit"))]
        public async Task<IActionResult> EditComment(int id)
        {
            var model =await _commentService.EditCommentAsync(id);
            return View(model);
        }
        /// <summary>
        /// Изменени комментария
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Comment/Edit")]
        public async Task<IActionResult> EditComment(CommentEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                await _commentService.EditCommentAsync(model);
                _logger.LogInformation($"Комментарий к статье {model.PostId} успешно изменен пользователем {model.UserId}");
                return RedirectToAction("ViewPost", "Post", new { @id = model.PostId });
            }
            _logger.LogError($"произошла ошибка при редактировании комментария {model.Id} к статье {model.PostId} пользователем {User.Identity.Name}");
            return View(model);
            
        }
    }
}
