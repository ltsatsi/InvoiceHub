using IMS_DomainLayer.Dtos.Account;
using IMS_DomainLayer.Mappers;
using IMS_DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        [HttpGet]
        public IActionResult SignIn(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInDto());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto signInDto, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(signInDto);

            var result = await _signInManager.PasswordSignInAsync(
                signInDto.Email, 
                signInDto.Password, 
                isPersistent: false, 
                lockoutOnFailure:   false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(signInDto);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignUpDto());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (!ModelState.IsValid)
                return View(signUpDto);

            var user = signUpDto.ToUserFromSignUp();
            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(signUpDto);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "Business");

            if (!addToRoleResult.Succeeded)
            {
                ViewBag.AddToRoleError = $"User {signUpDto.Email} created but failed to assign role.";
                return View(signUpDto);
            }

            ViewBag.SuccessMessage = $"User {signUpDto.Email} created successfully.";

            return RedirectToAction(nameof(SignIn));
        }




        [HttpPost]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
