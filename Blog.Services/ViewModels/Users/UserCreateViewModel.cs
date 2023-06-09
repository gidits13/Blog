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
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "123@example.ru")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль")]
        public string Password2 { get; set; }
        [Display(Name="Роли")]
        public List<RoleViewModel>? Roles { get; set; }
    }
}
