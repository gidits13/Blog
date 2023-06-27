using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ApiModels.Comments
{
    public class CommentAddApiModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}
