using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ApiModels.Tags
{
    public class TagApiModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
