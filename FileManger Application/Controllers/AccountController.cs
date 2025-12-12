using FileManger_Application.DTOs.UserDto;
using FileManger_Application.Exception;
using FileManger_Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace FileManger_Application.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserContract _userService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserContract userService, ILogger<AccountController> logger)
        {

            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<Result<UserResponse>>> Index()
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name == null)
            {
                return RedirectToAction("Login");
            }
            var userName = User.Identity.Name;
            var user = await _userService.GetUserById(userName);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }
        [HttpGet]
        public async Task<ActionResult<Result<UserResponse>>> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login([FromForm] AddUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(u => u.Errors.Select(o => o.ErrorMessage).ToList());

                var error = Result<UserResponse>.Fail(string.Join(";", errors), ErrorType.Validation);
                return View(error);
            }

            return View("index");
        }
    }
}
