using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        // UserManager is used to manage users in ASP.NET Core Identity
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
                return;
            }

            if (password.Length < 6)
                ModelState.AddModelError("Password", "Password must be at least 6 characters long.");

            if (!password.Any(char.IsUpper))
                ModelState.AddModelError("Password", "Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                ModelState.AddModelError("Password", "Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                ModelState.AddModelError("Password", "Password must contain at least one digit.");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                ModelState.AddModelError("Password", "Password must contain at least one special character.");

            if (password.Distinct().Count() < 1)
                ModelState.AddModelError("Password", "Password must contain at least one unique character.");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            ValidatePassword(registerViewModel.Password);

            if (ModelState.IsValid) {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded)
                {
                    //assign the user to a role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                    if (roleIdentityResult.Succeeded)
                    {
                        //show success message
                        return RedirectToAction("Register");
                    }
                }

            }
               
            // If we reach here, something went wrong
            //show error messages
            return View();

        }


        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

                if (signInResult != null && signInResult.Succeeded)
                {
                    if (loginViewModel.ReturnUrl != null)
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    //show success message
                    return RedirectToAction("Index", "Home");
                }

            }


            //show error messages
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            //show success message
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            // This action can be used to show an access denied page
            return View();
        }
         
    }
}
