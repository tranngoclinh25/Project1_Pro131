using ASM.Areas.Admin.Models;
using ASM.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] 
    public class AccountController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public IActionResult AdminLogin(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null) return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
            HttpContext.Session.Remove("AccountId");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public async Task<IActionResult> AdminLogin(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                //if (ModelState.IsValid)
                //{


                    Account kh = _context.Accounts
                    .Include(p => p.Role)
                    .SingleOrDefault(p => p.Email.ToLower() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        _notyfService.Error("Tài khoản không hợp lệ");
                        
                    }
                    string pass = (model.Password.Trim());
                    // + kh.Salt.Trim()
                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.ErrorMessage = "Thông tin đăng nhập không đúng. Vui lòng thử lại.";

                        return View(model);
                    }
                    //đăng nhập thành công

                    //ghi nhận thời gian đăng nhập
                    kh.LastLogin = DateTime.Now;
                    _context.Update(kh);
                    await _context.SaveChangesAsync();


                    var taikhoanID = HttpContext.Session.GetString("AccountId");
                    //identity
                    //luuw seccion Makh
                    HttpContext.Session.SetString("AccountId", kh.AccountId.ToString());

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("AccountId", kh.AccountId.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);


                    _notyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });

                //}
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }

            return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
        }
        [Route("logout.html", Name = "Logout")]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
        }

    }
}
