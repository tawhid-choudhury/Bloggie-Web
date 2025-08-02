using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories
{
    public interface IAdminUsersRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllUsers();
    }
}
