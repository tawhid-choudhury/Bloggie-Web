using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bdbc;

        public BlogPostRepository(BloggieDbContext bdbc)
        {
            this.bdbc = bdbc;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bdbc.BlogPosts.AddAsync(blogPost);
            await bdbc.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var blogExists = await bdbc.BlogPosts.FindAsync(id); // Find the blog post by ID
            if (blogExists != null)
            {
                bdbc.BlogPosts.Remove(blogExists); // Remove the blog post from the DbSet
                await bdbc.SaveChangesAsync(); // Save changes to the database
                return blogExists; // Return the deleted blog post
            }
            return null; // Return null if the blog post was not found
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            //use the DbContext to retrieve all blog posts
             return await bdbc.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await bdbc.BlogPosts
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var blogpostExists = await bdbc.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (blogpostExists != null) 
            {
                blogpostExists.Id = blogPost.Id; // Ensure the ID is set correctly
                blogpostExists.Heading = blogPost.Heading;
                blogpostExists.PageTitle = blogPost.PageTitle;
                blogpostExists.Content = blogPost.Content;
                blogpostExists.ShortDescription = blogPost.ShortDescription;
                blogpostExists.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                blogpostExists.UrlHandle = blogPost.UrlHandle;
                blogpostExists.PublishedDate = blogPost.PublishedDate;
                blogpostExists.Author = blogPost.Author;
                blogpostExists.Visible = blogPost.Visible;
                blogpostExists.Tags = blogPost.Tags; // Update the tags


                await bdbc.SaveChangesAsync();
                return blogpostExists;
            }
            else
            {
                return null; // Blog post not found
            }
        }
    }
}
