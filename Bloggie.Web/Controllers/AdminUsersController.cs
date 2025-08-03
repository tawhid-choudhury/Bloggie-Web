using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly IAdminUsersRepository adminUsersRepository;

        public AdminUsersController(IAdminUsersRepository adminUsersRepository)
        {
            this.adminUsersRepository = adminUsersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await adminUsersRepository.GetAllUsers();
            var adminUsersViewModel = new AdminUsersViewModel
            {
                Users = new List<User>()
            };


            foreach (var user in users)
            {
                adminUsersViewModel.Users.Add(new User
                {
                    UserID = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    Email = user.Email
                });
            }

            return View(adminUsersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdminUsersViewModel request) 
        {
            return null;
        }

    }
}
