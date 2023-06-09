using Blog.DAL.Models;
using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("SetUp")]
        public async Task<IActionResult> SetUp()
        {
            await _homeService.SetUp();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("Home/Error")]
        public IActionResult Error(int? statusCode=null)
        {
            if(statusCode.HasValue)
            {
                if (statusCode==404)
                    return View(statusCode.ToString());
            }
            return View("500");
        }
    }
}
