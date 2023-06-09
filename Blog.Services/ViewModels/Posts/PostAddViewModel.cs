using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.DAL.Models;
using Blog.Services.ViewModels.Tags;

namespace Blog.Services.ViewModels.Posts
{
    public class PostAddViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<TagViewModel>? Tags { get; set; }

        [Required(ErrorMessage ="Обязательное поле")]
        [Display(Name ="Title")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Обязательное поле")]
        [Display(Name ="Текст статьи")]
        public string Text { get; set; }
    }
}
