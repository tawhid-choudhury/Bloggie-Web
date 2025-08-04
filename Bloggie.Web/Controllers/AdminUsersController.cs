using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IAdminUsersRepository adminUsersRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IAdminUsersRepository adminUsersRepository, UserManager<IdentityUser> userManager)
        {
            this.adminUsersRepository = adminUsersRepository;
            this.userManager = userManager;
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
        public async Task<IActionResult> List(AdminUsersViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if (identityResult != null)
            {
                if (identityResult.Succeeded)
                {
                    var roles = new List<string>{"User"};
                    if (request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }
                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult != null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List");
                    }


                }

            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Guid id) 
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user is not null) 
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List");
                }
            }
            return View();
        }
    }
}
