using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Tags
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
