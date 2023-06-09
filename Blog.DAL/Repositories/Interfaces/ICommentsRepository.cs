using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetCommentsByPostAsync(int id);
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
        Task EditCommentAsync(Comment comment);
    }
}
