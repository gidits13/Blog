﻿using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ViewModels.Comments
{
    public class CommentsViewModel
    {
        public List<Comment> Comments { get; set; }
    }
}
