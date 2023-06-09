using Blog.DAL.Models;
using Blog.Services.ViewModels.Roles;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Users
{
    public class UserEditAdminModeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        public List<RoleViewModel> Roles { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
