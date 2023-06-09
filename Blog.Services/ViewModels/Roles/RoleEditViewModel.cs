using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Roles
{
    public class RoleEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
