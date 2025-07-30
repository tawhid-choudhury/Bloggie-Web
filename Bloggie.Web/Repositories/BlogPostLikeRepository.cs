
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bdbc;

        public BlogPostLikeRepository(BloggieDbContext bdbc)
        {
            this.bdbc = bdbc;
        }

        public async Task<BlogPostLike> AddLikeToBlog(BlogPostLike blogPostLike)
        {
            await bdbc.BlogPostLikes.AddAsync(blogPostLike);
            await bdbc.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
           return await bdbc.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();

        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            var count = await bdbc.BlogPostLikes.CountAsync( x => x.BlogPostId == blogPostId);

            return count;
        }
    }
}
