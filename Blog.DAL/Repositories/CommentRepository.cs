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
    public class CommentRepository : ICommentsRepository
    {
        private readonly DBContext _dbContext;

        public CommentRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task AddCommentAsync(Comment comment)
        {
                _dbContext.Comments.Add(comment);
                await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditCommentAsync(Comment comment)
        {
            _dbContext.Comments.Update(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _dbContext.Comments.FirstOrDefaultAsync(c=>c.Id==id);
            

        }

        public async Task<List<Comment>> GetCommentsByPostAsync(int id)
        {
            return await _dbContext.Comments.Where(c=>c.PostId==id).Include(c=>c.User).ToListAsync();
        }
    }
}
