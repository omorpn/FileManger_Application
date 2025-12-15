using FileManger_Application.DTOs.UserDto;
using FileManger_Application.Exception;
using FileManger_Application.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileManger_Application.Controllers
{
    [Controller]
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserContract _userService;

        public UserController(UserContract userService)
        {
            _userService = userService;
        }
        [HttpGet("/dashboard")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<Result<UserResponse>>> Dashboard()
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name == null)
            {
                return LocalRedirect("account/login");
            }
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByEmail(userName);
            if (user == null)
            {
                return LocalRedirect("account/login");
            }

            // _logger.Log(LogLevel.Information, "Login successful");
            return View(user.Data);
        }




    }
}
