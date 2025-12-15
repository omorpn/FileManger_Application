using FileManger_Application.Model;
using FileManger_Application.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileManger_Application.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly FileContract _file;
        private readonly UserManager<ApplicationUser> _user;
        public FilesController(FileContract fileContract, UserManager<ApplicationUser> user)
        {
            _file = fileContract;
            _user = user;
        }
        [HttpGet("/files")]
        public async Task<ActionResult> Files()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            var userEmail = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return BadRequest();

            }

            var user = await _user.FindByEmailAsync(userEmail);
            if (user is null)
            {
                return BadRequest();
            }
            var roles = await _user.GetRolesAsync(user);
            if (roles.Contains("ADMIN"))
            {
                var file = await _file.GetAllFilesAsync();
                return View(file.Data);
            }
            var files = await _file.GetFileByUserIdAsync(user.Id);
            return View(files.Data);

        }
    }
}
