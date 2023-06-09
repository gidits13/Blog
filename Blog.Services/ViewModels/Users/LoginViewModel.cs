using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blog.Services.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string Password { get; set; }
    }
}
