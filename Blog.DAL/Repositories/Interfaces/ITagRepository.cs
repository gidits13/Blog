using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task AddTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(Tag tag);
        Task<List<Tag>> GetAllTagsAsync();

        Task<Tag> GetTagAsync(int id);
    }   
}
