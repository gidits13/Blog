using Blog.Services.ApiModels.Tags;
using Blog.Services.ViewModels.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ApiModels.Posts
{
    public class PostEditApiModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Текст статьи")]
        public string Text { get; set; }
        public List<TagApiModel>? Tags { get; set; }
    }
}
