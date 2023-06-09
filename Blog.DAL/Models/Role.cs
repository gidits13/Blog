using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Role : IdentityRole<int>, IEntityTypeConfiguration<Role>
    {
       
       public List<User>? Users { get; set; }
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
/*            builder.HasData(new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                            new Role { Id = 2, Name = "Moderator", NormalizedName = "MODERATOR" },
                            new Role { Id = 3, Name = "User", NormalizedName = "USER" });*/
            //builder.HasMany(x => x.Users).WithMany(x=>x.Roles).UsingEntity("UserRoles");
        }
    }
}
