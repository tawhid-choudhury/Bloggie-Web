using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository itr;

        public AdminBlogPostsController(ITagRepository itr)
        {
            this.itr = itr;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get all tags from repository
            List<Tag> tags = (await itr.GetAllAsync()).ToList();

            AddBlogPostRequest model = new AddBlogPostRequest 
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value= x.Id.ToString() })
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest model)
        {
            return RedirectToAction("Add");
        }
    }
}
