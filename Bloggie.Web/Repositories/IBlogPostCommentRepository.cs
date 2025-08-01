using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddCommentAsync(BlogPostComment comment);
        Task<IEnumerable<BlogPostComment>> GetAllCommentsByBlogIdAsync(Guid blogId);
    }
}
