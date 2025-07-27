using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository bpr;

        public BlogsController(IBlogPostRepository bpr)
        {
            this.bpr = bpr;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogpost = await bpr.GetByUrlHandleAsync(urlHandle);
            if (blogpost != null)
            {
                return View(blogpost);
            }

            return View();
        }
    }
}
