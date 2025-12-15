
using FileManger_Application.DTOs.Admin;
using FileManger_Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace FileManger_Application.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly UserContract _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly FileContract _file;
        public AdminController(UserContract userService, ILogger<AccountController> logger, FileContract file)
        {
            _userService = userService;
            _logger = logger;
            _file = file;
        }
        [HttpGet("/admin")]
        public async Task<IActionResult> Dashboard()
        {
            var userCountResult = await _userService.GetAllUsers();
            var recentUser = await _userService.Search(DateTime.Now.AddDays(-7).ToString(), 7, 1);

            var fileCountResult = await _file.GetAllFilesAsync();

            var data = new AdminDashboardDto
            {
                TotalUsers = userCountResult.Data.Count,
                TotalFiles = fileCountResult.Data.Count,
                TotalStorageUsedInMB = Math.Round(fileCountResult.Data.Where(u => u.Size.HasValue).Sum(f => f.Size.Value)),
                RecentUsers = recentUser.Data


            };

            return View(data);
        }
    }
}
