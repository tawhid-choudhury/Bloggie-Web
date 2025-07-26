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
        private readonly IBlogPostRepository bpr;

        public AdminBlogPostsController(ITagRepository itr, IBlogPostRepository bpr)
        {
            this.itr = itr;
            this.bpr = bpr;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get all tags from repository
            List<Tag> tags = (await itr.GetAllAsync()).ToList();

            AddBlogPostRequest model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //map view model to domain model
            BlogPost blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
                //map selected tags to domain model
                Tags = new List<Tag>()

            };
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await itr.GetAsync(selectedTagIdAsGuid);


                if (existingTag != null)
                {
                    blogPost.Tags.Add(existingTag);
                }
            }
            //add blog post to repository
            await bpr.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //call repository to get all blog posts
            var blogPosts = await bpr.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //retrieve respective blog post and tags from repository
            var blogPost = await bpr.GetByIdAsync(id);
            var tagsDomainModel = await itr.GetAllAsync();

            //check if blog post exists
            if (blogPost != null)
            {
                //map domain model to view model
                var model = new EditBlogPostRequest
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
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                //pass the view model to the view
                return View(model);

            }



            //if blog post does not exist, return null
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model to domain model
            BlogPost blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
                Tags = new List<Tag>()
            };
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTagId, out var selectedTagIdAsGuid))
                {
                    var existingTag = await itr.GetAsync(selectedTagIdAsGuid);
                    if (existingTag != null)
                    {
                        blogPostDomainModel.Tags.Add(existingTag);
                    }
                }
            }
            //update blog post in repository
            var updatedBlog = await bpr.UpdateAsync(blogPostDomainModel);
            if (updatedBlog != null)
            {
                //show success message
                return RedirectToAction("List");
            }
            //if update fails, show error message
            return RedirectToAction("Edit");



        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
//talk to repository to delete blog post
            var deletedBlogPost = await bpr.DeleteAsync(editBlogPostRequest.Id);
            if (deletedBlogPost != null)
            {
                //show success message
                return RedirectToAction("List");
            }
            //if delete fails, show error message
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });

        }
    }
}
