using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task AddPostAsync(Post post);
        Task DeletePostAsync(Post post);
        Task<List<Post>> GetAllPostsAsync();
        Task EditPostAsync(Post post);
        Task<Post> GetPostByIdAsync(int id);
    }
}
