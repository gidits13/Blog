using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Comments
{
    public class CommentAddViewModel
    {
        [Required(ErrorMessage ="Обязательное поле")]
        [Display(Name ="Комментарий")]
        public string Text { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
