using AutoMapper;
using Blog.DAL.Models;
using Blog.DAL.Repositories.Interfaces;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        public TagService(IMapper mapper, ITagRepository tagRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        public async Task AddTag(TagAddViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);
            await _tagRepository.AddTagAsync(tag);
        }

        public async Task DeleteTag(int id)
        {
            var tag = await _tagRepository.GetTagAsync(id);
            await _tagRepository.DeleteTagAsync(tag);
        }

        public async Task EditTag(TagEditViewModel model)
        {
            var tag = await _tagRepository.GetTagAsync(model.Id);
            tag.Name = model.Name;
            await _tagRepository.UpdateTagAsync(tag);
        }

        public async Task<TagEditViewModel> EditTag(int id)
        {
            var tag = await _tagRepository.GetTagAsync(id);
            var model = new TagEditViewModel { Name = tag.Name };
            return model;
        }

        public async Task<List<Tag>> GetTags()
        {
            return await _tagRepository.GetAllTagsAsync();
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await _tagRepository.GetTagAsync(id);
        }
    }
}
