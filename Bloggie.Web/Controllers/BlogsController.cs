using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository bpr;
        private readonly IBlogPostLikeRepository bpLikeRepository;
        private readonly IBlogPostCommentRepository bpCommentRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(
            IBlogPostRepository bpr,
            IBlogPostLikeRepository bpLikeRepository,
            IBlogPostCommentRepository bpCommentRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.bpr = bpr;
            this.bpLikeRepository = bpLikeRepository;
            this.bpCommentRepository = bpCommentRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await bpr.GetByUrlHandleAsync(urlHandle);

            if (blogPost != null)
            {
                var totalLikes = await bpLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    //Get Like For this blog for this user
                    var likesForBlog = await bpLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                //Get Comments for this blog
                var blogPostCommentsDomainModel = await bpCommentRepository.GetAllCommentsByBlogIdAsync(blogPost.Id);

                //Map Comments to ViewModel
                var blogCommentsForView = new List<BlogPostCommentViewModel>();

                foreach (var comment in blogPostCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogPostCommentViewModel
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        Username = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName,
                    });
                }

                var blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    BlogPostComments = blogCommentsForView
                };



                return View(blogDetailsViewModel);

            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    DateAdded = DateTime.Now,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                };

                await bpCommentRepository.AddCommentAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }
            return View();
        }
    }
}
