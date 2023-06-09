using Blog.DAL.Models;
using Blog.Services.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Interfaces
{
    public interface IPostService
    {
        Task AddPostAsync(PostAddViewModel model);
        Task DeletePostAsync(int id);
        Task EditPostAsync(PostEditViewModel model, int id);
        Task<PostEditViewModel> EditPostAsync(int id);
        Task<PostsViewModel> GetAllPostsAsync();
        Task<PostViewModel>GetPostByIdAsync(int id);
    }
}
