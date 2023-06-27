using Blog.DAL.Models;
using Blog.Services.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentAddViewModel model);
        Task EditCommentAsync(CommentEditViewModel model);
        Task<CommentEditViewModel> EditCommentAsync(int id);
        Task DeleteCommentAsync(int id);
        Task<List<Comment>> GetCommentsByPostAsync(int postId);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<Comment> GetCommentByIdAsyncAsNoTracking(int id);
    }
}
