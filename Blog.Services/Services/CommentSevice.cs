using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repositories.Interfaces;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Comments;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class CommentSevice : ICommentService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IMapper _mapper;   
        private readonly IPostRepository _postRepository;
        private readonly UserManager<User> _userManager;

        public CommentSevice(ICommentsRepository commentsRepository, IMapper mapper,IPostRepository postRepository, UserManager<User> userManager)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public async Task AddCommentAsync(CommentAddViewModel model)
        {
            var post = await _postRepository.GetPostByIdAsync(model.PostId);
            var comment = _mapper.Map<Comment>(model);
            comment.Post = post;
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            comment.User = user;
            await _commentsRepository.AddCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _commentsRepository.GetCommentByIdAsync(id);
            await _commentsRepository.DeleteCommentAsync(comment);
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _commentsRepository.GetCommentByIdAsync(id);
        }

        public async Task<List<Comment>> GetCommentsByPostAsync(int postId)
        {
            return await GetCommentsByPostAsync(postId);
        }

        public async Task EditCommentAsync(CommentEditViewModel model)
        {
            var comment = _mapper.Map<Comment>(model);
            await _commentsRepository.EditCommentAsync(comment);
        }

        public async Task<CommentEditViewModel> EditCommentAsync(int id)
        {
            var comment = await _commentsRepository.GetCommentByIdAsync(id);
            var model = new CommentEditViewModel {Id=comment.Id ,PostId = comment.PostId, Text = comment.Text, UserId = comment.userId };
            return model;
        }
    }
}
