using FileManger_Application.DTOs.UserDto;
using FileManger_Application.ServiceContract;
using FileManger_Application.Views.ViewsDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileManger_Application.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserContract _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly FileContract
            _fileContract;
        public AccountController(UserContract userService, ILogger<AccountController> logger, FileContract fileContract)
        {

            _userService = userService;
            _logger = logger;
            _fileContract = fileContract;
        }

        [HttpGet]
        public ActionResult Login()
        {
            var referer = HttpContext.Request.Headers.Referer;
            ViewBag.referer = referer;
            return View();
        }
        public async Task<ActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            var result = await _userService.LoginAsync(login.Email, login.Password, login.RememberMe);
            if (!result.Success)
            {
                return View(result.Data);
            }
            return LocalRedirect("/account");
        }
        [HttpGet]
        public async Task<ActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("/");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(AddUserRequest register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            var result = await _userService.Create(register);
            if (!result.Success)
            {
                return View(result.Data);
            }
            var refer = HttpContext.Request.Headers.Referer;
            return LocalRedirect(refer.ToString() ?? "/dashboard");
        }
        [HttpGet]
        public async Task<ActionResult> ForgottenPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgottenPassword(string email)
        {
            await _userService.ForgottenPasswordAsync(email);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
            {
                return View();
            }
            ResetPassword reset = new ResetPassword() { Email = email, Token = token };
            return View(reset);
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPassword reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }
            await _userService.ResetPasswordAsync(reset.Email, reset.NewPassword, reset.Token);
            return View();

        }
        public async Task<ActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return LocalRedirect("/");
        }
    }
}
