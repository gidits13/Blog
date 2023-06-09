using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Tag : IEntityTypeConfiguration<Tag>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }

        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(x => x.Id);
        }
    }
}
