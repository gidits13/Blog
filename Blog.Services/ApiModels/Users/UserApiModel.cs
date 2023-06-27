using Blog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.ApiModels.Users
{
    public class UserApiModel
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Email Пользователя
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Спиок ролей пользователя
        /// </summary>
        public List<string> Roles { get; set; }
    }
}
