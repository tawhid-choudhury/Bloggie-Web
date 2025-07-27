using System.Diagnostics;
using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository bpr;
        private readonly ITagRepository tr;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository bpr, ITagRepository tr)
        {
            _logger = logger;
            this.bpr = bpr;
            this.tr = tr;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await bpr.GetAllAsync();
            var tags = await tr.GetAllAsync();

            var homeViewModel = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
