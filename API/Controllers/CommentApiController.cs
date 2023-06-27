using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.ApiModels.Comments;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService _commentsService;
        private readonly ILogger<CommentApiController> _logger;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CommentApiController(ICommentService commentsService, ILogger<CommentApiController> logger, IPostService postService,
            IMapper mapper, UserManager<User> userManager)
        {
            _commentsService = commentsService;
            _logger = logger;
            _postService = postService;
            _mapper = mapper;
            _userManager= userManager;
        }
        /// <summary>
        /// Получение списка комментариев к статье
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetPostComments")]
        public async Task<IActionResult> GetPostComments(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post != null) 
            {
                var comments = await _commentsService.GetCommentsByPostAsync(id);
                var response = _mapper.Map<List<CommentApiModel>>(comments);
                return StatusCode(200, response);
            }
            return StatusCode(400);
        }
        /// <summary>
        /// Добавление комментария к статье
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment(CommentAddApiModel model)
        {
            if(ModelState.IsValid)
            {
                var post = await _postService.GetPostByIdAsync(model.PostId);
                if(post!=null)
                {
                    var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                    var comment = new CommentAddViewModel { Text = model.Text, PostId = model.PostId, UserId = user.Id };
                    await _commentsService.AddCommentAsync(comment);
                    _logger.LogInformation($"Комментарий к статье с id {model.PostId} добавлен пользователем {user.Id} {user.Name} {user.LastName}");
                    return StatusCode(200, $"Комментарий к статье с id {model.PostId} добавлен пользователем {user.Id} {user.Name} {user.LastName}");
                }
                _logger.LogError($"Ошибка добавления комментария к статье {model.PostId}, статья не существует");
                return StatusCode(400, $"Ошибка добавления комментария к статье {model.PostId}, статья не существует");
            }
            _logger.LogError($"Ошибка добавления комментария к статье {model.PostId}");
            return StatusCode(400, $"Ошибка добавления комментария к статье {model.PostId}");
        }
        /// <summary>
        /// Удаление комментария по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles ="Admin")]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);
            if(comment!=null)
            {
                await _commentsService.DeleteCommentAsync(id);
                _logger.LogInformation($"Комментарий c ID {comment.Id} к статье {comment.PostId} удален");
                return StatusCode(200, $"Комментарий c ID {comment.Id} к статье {comment.PostId} удален");
            }
            _logger.LogError($"ошибка удаления комметария c ID {id}, комментарий не существует");
            return StatusCode(400, $"ошибка удаления комметария c ID {id}, комментарий не существует");
        }
        /// <summary>
        /// Редактирование комментария
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles ="Admin")]
        [Route("EditComment")]
        public async Task<IActionResult> EditComment(CommentEditApiModel model)
        {
            if(ModelState.IsValid)
            {
                var comment =await _commentsService.GetCommentByIdAsyncAsNoTracking(model.Id);
                if(comment!=null)
                {
                    var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                    var cmodel = new CommentEditViewModel { Id=comment.Id, PostId=comment.PostId, Text=model.Text, UserId=comment.userId };
                    
                    await _commentsService.EditCommentAsync(cmodel);
                    _logger.LogInformation($"Комментарий с ID {comment.Id} к статье {comment.Post} изменен пользователем {user.Name} {user.LastName}");
                    return StatusCode(200, $"Комментарий с ID {comment.Id} к статье {comment.Post} изменен пользователем {user.Name} {user.LastName}");
                }
                _logger.LogError($"Ошибка редактирования комментария, комментарий c ID {model.Id} не существует");
                return StatusCode(400, $"Ошибка редактирования комментария, комментарий c ID {model.Id} не существует");
            }
            return StatusCode(400, $"Ошибка редактирования комментария");
        }
    }
}
