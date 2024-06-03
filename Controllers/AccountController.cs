using Demo.DAL.Models;
using Graduation.PL.Servises.EmailServises;
using Graduation.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Graduation.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSetting _emailSetting;

        public AccountController(UserManager<ApplicationUser> userManager
                                , SignInManager<ApplicationUser> signInManager,
                                 IEmailSetting emailSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSetting = emailSetting;
        }


        public IActionResult SignUp()

        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel userVM) //Register
        {

            if (ModelState.IsValid)  //Server Side Validation
            {
                var User = await _userManager.FindByNameAsync(userVM.Username);
                if (User is null)
                {
                    User = new ApplicationUser()
                    {
                        Fname = userVM.Fname,
                        Lname = userVM.Lname,
                        UserName = userVM.Username,
                        Email = userVM.Email,
                        IsAgree = userVM.IsAgree,
                    };

                    var result = await _userManager.CreateAsync(User, userVM.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    foreach (var Eror in result.Errors)
                        ModelState.AddModelError(string.Empty, Eror.Description);
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "User Name IS Exsisit");
                }

            }


            return View(userVM);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel Model) // Login
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(Model.Email);
                if (User is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(User, Model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(User, Model.Password, Model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }
                    }

                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(Model);
        }

        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(LogIn));
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmailResetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    TempData["GenerateToken"] = token;
                    var URl = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = token }, "https", "localhost:44360");
                    await _emailSetting.SendEmailAsyn(
                        to: model.Email,
                        Subject: "Reset Your  Password",
                        body: URl); //bode url to to get view to change Password

                    return RedirectToAction(nameof(ChackYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invaild Email");

            }
            return View(model);
        }

        public IActionResult ChackYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string Email, string Token)
        {
            TempData["Email"] = Email;
            TempData["Token"] = Token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetpasswordViewModel model)
        {
            string Email = TempData["Email"] as string;
            string Token = TempData["Token"] as string;
            if (ModelState.IsValid && TempData["GenerateToken"] as string == Token)
            {
                var user = await _userManager.FindByEmailAsync(Email);

                var result = await _userManager.ResetPasswordAsync(user, Token, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(LogIn));
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                ModelState.AddModelError(string.Empty, "An Erorr When You Reset Password ,Try Again");
            }
            return View(model);
        }
    }
}
