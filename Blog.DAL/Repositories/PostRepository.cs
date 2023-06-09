using Blog.DAL.Models;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DBContext _dbContext;

        public PostRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task AddPostAsync(Post post)
        {
            _dbContext.Add(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            _dbContext.Remove(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditPostAsync(Post post)
        {
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Post>> GetAllPostsAsync()
        {
            return _dbContext.Posts.Include(p=>p.Tags).Include(p=>p.User).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _dbContext.Posts.Include(p=>p.Tags).Include(p=>p.User).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
