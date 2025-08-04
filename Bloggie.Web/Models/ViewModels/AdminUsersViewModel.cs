using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class AdminUsersViewModel
    {
        public List<User> Users { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public  bool AdminRoleCheckbox { get; set; }
    }
}
