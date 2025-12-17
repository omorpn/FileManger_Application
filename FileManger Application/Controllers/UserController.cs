using FileManger_Application.DTOs.UserDto;
using FileManger_Application.Exception;
using FileManger_Application.ServiceContract;
using FileManger_Application.Views.ViewsDTOs;
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
        private readonly FileContract _files;
        private readonly ILogger<UserController> _logger;

        public UserController(UserContract userService, FileContract files, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
            _files = files;
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
            var userFiles = await _files.GetFileByUserIdAsync(user.Data.UserId);
            if (userFiles == null)
            {
                var res = new UserDashboardViewModel();
                return View(res);
            }
            var viewModel = new UserDashboardViewModel
            {
                UserName = user.Data.FirstName,
                TotalFiles = userFiles.Data.Count,
                StorageUsed = Convert.ToUInt32(userFiles.Data.Sum(f => f.Size)),
                RecentFiles = userFiles.Data.OrderByDescending(f => f.CreatedAt).Take(5).ToList(),

            };
            _logger.Log(LogLevel.Information, "Login successful");
            return View(viewModel);
        }
        [HttpGet]
        public async Task<ActionResult<Result<UserResponse>>> Settings()
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
