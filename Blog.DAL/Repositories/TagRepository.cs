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
    public class TagRepository : ITagRepository
    {
        private readonly DBContext _dbcontext;

        public TagRepository(DBContext dBContext)
        {
            _dbcontext = dBContext;
        }

        public async Task AddTagAsync(Tag tag)
        {
           _dbcontext.Tags.Add(tag);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(Tag tag)
        {
            _dbcontext?.Tags.Remove(tag);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dbcontext.Tags.Include(t=>t.Posts).ToListAsync();
        }

        public async Task<Tag> GetTagAsync(int id)
        {
            return await _dbcontext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _dbcontext.Tags.Update(tag);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
