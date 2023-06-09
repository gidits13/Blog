using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class User : IdentityUser<int> , IEntityTypeConfiguration<User>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Role>? Roles { get; set; }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(t => t.Id);
            
        }
    }
}
