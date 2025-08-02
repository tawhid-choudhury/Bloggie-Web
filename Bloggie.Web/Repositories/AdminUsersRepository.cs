using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class AdminUsersRepository : IAdminUsersRepository
    {
        private readonly AuthDbContext adbc;

        public AdminUsersRepository(AuthDbContext adbc)
        {
            this.adbc = adbc;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            var users = await adbc.Users.ToListAsync();

            var superAdmin = await adbc.Users.FirstOrDefaultAsync(u => u.Email == "superadmin@bloggie.com");

            //if (superAdmin is not null) // Using "is not null" for avoiding null checks
            if (superAdmin != null)
            {
                users.Remove(superAdmin);
            }

            return users;
        }
    }
}
