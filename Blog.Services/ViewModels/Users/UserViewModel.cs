using Blog.DAL.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
        public string Password { get; set; }
    }
}
