using Blog.Services.Services.Interfaces;
using Blog.Services.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;

namespace Blog.App.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Authorize]
        [Route("Tags")]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagService.GetTags();
            var model = new TagsViewModel { Tags = tags };
            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("Tag/Add")]
        public IActionResult AddTag()
        {
            var model = new TagAddViewModel();
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("Tag/Add")]
        public async Task<IActionResult> AddTag(TagAddViewModel model)
        {
            await _tagService.AddTag(model);
            return RedirectToAction("GetTags", "Tag");
        }
        [HttpGet]
        [Authorize]
        [Route("Tag/Delete")]
        public async Task<IActionResult> deleteTag(int id)
        {
            await _tagService.DeleteTag(id);
            return RedirectToAction("GetTags", "Tag");
        }
        [HttpGet]
        [Authorize]
        [Route("Tag/Edit")]
        public async Task<IActionResult> EditTag(int id)
        {
            var model =await _tagService.EditTag(id);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [Route("Tag/Edit")]
        public async Task<IActionResult> EditTag(TagEditViewModel model)
        {
            await _tagService.EditTag(model);
            return RedirectToAction("GetTags", "Tag");
        }
    }
}
