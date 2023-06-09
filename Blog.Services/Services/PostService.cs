using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repositories.Interfaces;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Posts;
using Blog.Services.ViewModels.Tags;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class PostService : IPostService
    {
       // private readonly UserManager<User> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICommentsRepository _commentRepository;

        public PostService(IPostRepository postRepository, IMapper mapper, ITagRepository tagRepository, UserManager<User> userManager, ICommentsRepository commentsRepository)
        {
            //_userManager = userManager;
            _postRepository = postRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
            _userManager = userManager;
            _commentRepository = commentsRepository;
        }

        public async Task AddPostAsync(PostAddViewModel model)
        {
            var postTags = new List<Tag>();
            if(model.Tags!=null)
            {
                var CheckedTags = model.Tags.Where(t=>t.IsChecked==true).Select(t=>t.Id).ToList();
                postTags = await _tagRepository.GetAllTagsAsync();
                postTags=postTags.Where(t=> CheckedTags.Contains(t.Id)).ToList();
            }
            var post = _mapper.Map<Post>(model);
            post.Tags = postTags;
            post.UserId = model.UserId;
            await _postRepository.AddPostAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post != null) {await _postRepository.DeletePostAsync(post); }
        }

        public async Task EditPostAsync(PostEditViewModel model, int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            post.Text = model.Text;
            post.Title=model.Title;
            var postTags = new List<Tag>();
            if(model.Tags!=null)
            {
                var CheckedTags = model.Tags.Where(t => t.IsChecked == true).Select(t => t.Id).ToList();
                postTags = await _tagRepository.GetAllTagsAsync();
                postTags = postTags.Where(t => CheckedTags.Contains(t.Id)).ToList();
            }
            post.Tags = postTags;
            await _postRepository.EditPostAsync(post);
        }
        public async Task<PostEditViewModel> EditPostAsync(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var tags = await _tagRepository.GetAllTagsAsync();
            var tagsViews = tags.Select(t=>new TagViewModel() { Id=t.Id,Name=t.Name}).ToList();

            foreach (var tagview in tagsViews) 
            {
                foreach (var postTag in post.Tags)
                {
                    if(postTag.Id ==tagview.Id)
                    {
                        tagview.IsChecked=true;
                        break;
                    }    
                }
            }
            var model = _mapper.Map<PostEditViewModel>(post);
            model.Tags = tagsViews;
            return model;
        }

        public async Task<PostsViewModel> GetAllPostsAsync()
        {
            var posts =  await _postRepository.GetAllPostsAsync();
            PostsViewModel model = new PostsViewModel();
            model.Posts = posts;
            return model;
        }

        public async Task<PostViewModel> GetPostByIdAsync(int id)
        {
            var post = await  _postRepository.GetPostByIdAsync(id);
            PostViewModel model = new PostViewModel();
            model = _mapper.Map<PostViewModel>(post);
            var comments = await _commentRepository.GetCommentsByPostAsync(model.Id);
            model.Comments = comments;
            return model;
        }
    }
}
