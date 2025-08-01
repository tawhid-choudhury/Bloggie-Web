using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostComment> AddCommentAsync(BlogPostComment comment)
        {
            await bloggieDbContext.BlogPostComments.AddAsync(comment);
            await bloggieDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetAllCommentsByBlogIdAsync(Guid blogPostId)
        {
            var BlogPost = await bloggieDbContext.BlogPostComments.Where(c => c.BlogPostId== blogPostId).ToListAsync();
            return BlogPost;
        }
    }
}
