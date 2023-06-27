using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Blog.App.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeService _homeService;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(IHomeService homeService, ILogger<HomeController> logger)
        {
            _homeService = homeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Генерация начальных пользователей и ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SetUp")]
        public async Task<IActionResult> SetUp()
        {
            await _homeService.SetUp();
            _logger.LogInformation($"Созданы базовые учетные записи и Роли");
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Обработака http статусов ошибок
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Home/Error")]
        public IActionResult Error(int? statusCode=null)
        {
            if(statusCode.HasValue)
            {
                if (statusCode==404)
                    _logger.LogError($"Произошла ошибка -  {statusCode.Value.ToString()}");
                    return View(statusCode.ToString());
            }
            return View("500");
        }
    }
}
