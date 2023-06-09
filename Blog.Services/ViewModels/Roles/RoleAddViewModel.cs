using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Roles
{
    public class RoleAddViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
