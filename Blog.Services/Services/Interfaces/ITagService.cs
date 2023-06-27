using Blog.DAL.Models;
using Blog.Services.ViewModels.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Interfaces
{
    public interface ITagService
    {
        Task AddTag(TagAddViewModel model);
        Task EditTag(TagEditViewModel model);
        Task<TagEditViewModel> EditTag(int id);
        Task DeleteTag(int id);
        Task<List<Tag>>GetTags();
        Task<Tag>GetTagById(int id);
    }
}
