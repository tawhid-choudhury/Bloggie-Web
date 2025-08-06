using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        // This is the DbContext that will be used to interact with the database
        private readonly BloggieDbContext bdbc;

        // Constructor injection of the DbContext
        public TagRepository(BloggieDbContext bdbc)
        {
            this.bdbc = bdbc;
        }

        // This method adds a new tag to the database
        public async Task<Tag> AddAsync(Tag t)
        {
            await bdbc.Tags.AddAsync(t);
            await bdbc.SaveChangesAsync();
            return t;
        }

        // This method deletes a tag from the database by its ID
        public async Task<Tag?> DeleteAsync(Guid id)
        {
            Tag? existingTag = await bdbc.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bdbc.Tags.Remove(existingTag);
                await bdbc.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        // This method retrieves all tags from the database
        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery)
        {
            var query = bdbc.Tags.AsQueryable();

            //filter 
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
            }
            //sort

            //pagination

            return await query.ToListAsync();
            // return await bdbc.Tags.ToListAsync();

        }

        // This method retrieves a tag by its ID from the database
        public async Task<Tag?> GetAsync(Guid id)
        {
           return await bdbc.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        // This method updates an existing tag in the database
        public async Task<Tag?> UpdateAsync(Tag t)
        {
            Tag? existingTag = await bdbc.Tags.FindAsync(t.Id);

            if (existingTag != null)
            {
                existingTag.Name = t.Name;
                existingTag.DisplayName = t.DisplayName;

                await bdbc.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
