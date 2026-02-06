using HoatDongSinhVien.Models.Application_User;
using HoatDongSinhVien.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HoatDongSinhVien.Models.Data;

namespace HoatDongSinhVien.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly HoatDongSinhVienDbcontext _Context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            HoatDongSinhVienDbcontext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Home"); 
            }

            
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                    return View();
                }

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ!";
                return RedirectToAction("Index", "Home"); 
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
             
                TempData["ChangePasswordSweetAlert"] = "Đổi mật khẩu thành công!";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Đổi mật khẩu thất bại.";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessageForgotPass"] = "Dữ liệu không hợp lệ";
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || user.Email != model.Email)
            {
                TempData["ErrorMessageForgotPass"] = "Tên đăng nhập hoặc Email không đúng";
                return View(model);
            }


            TempData["ShowResetPasswordModal"] = true;
            TempData["ResetUserId"] = user.Id;

            return RedirectToAction("ForgotPassword"); 
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ResetPasswordError"] = "Dữ liệu không hợp lệ";
                TempData["ShowResetPasswordModal"] = true;
                TempData["ResetUserId"] = model.UserId;
                return RedirectToAction(nameof(ForgotPassword));
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                TempData["ResetPasswordError"] = "Người dùng không tồn tại";
                return RedirectToAction(nameof(ForgotPassword));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user, token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["ResetPasswordSweetAlert"] = "Đặt lại mật khẩu thành công!";
                return RedirectToAction(nameof(ForgotPassword));
            }

            TempData["ResetPasswordError"] =
                result.Errors.FirstOrDefault()?.Description ?? "Đặt lại mật khẩu thất bại";

            TempData["ShowResetPasswordModal"] = true;
            TempData["ResetUserId"] = model.UserId;
            return RedirectToAction(nameof(ForgotPassword));
        }


        public async Task<IActionResult> Logout()
        {
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
