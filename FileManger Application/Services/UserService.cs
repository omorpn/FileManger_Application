using FileManger_Application.DTOs.UserDto;
using FileManger_Application.Exception;
using FileManger_Application.Helpers;
using FileManger_Application.Model;
using FileManger_Application.ServiceContract;
using FileManger_Application.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FileManger_Application.Services
{
    public class UserService : UserContract
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> role, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signinManager = signInManager;
            _roleManager = role;
        }

        public async Task<Result<UserResponse>> Create(AddUserRequest user)
        {
            // Check user is null
            if (user == null)
            {
                return Result<UserResponse>.Fail("User can't be null", ErrorType.Validation);
            }
            var existed = await _userManager.FindByEmailAsync(user.Email);
            if (existed != null)
            {
                return Result<UserResponse>.Fail("Email already exist ", ErrorType.Validation);
            }

            ApplicationUser addUser = user.ToUser();
            var role = new[] { new ApplicationRole { Name = "USER" },
            new ApplicationRole { Name = "ADMIN" }
            };
            foreach (var r in role)
            {
                var roleExist = await _roleManager.RoleExistsAsync(r.Name);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(r);
                }
            }

            var result = await _userManager.CreateAsync(addUser, user.Password);
            _ = await _userManager.AddToRoleAsync(addUser, "USER");
            await _signinManager.SignInAsync(addUser, false);
            return ResponseResult.FromIdentityResult(result, "User created successfully", addUser.ToUserResponse(), ErrorType.Validation);
        }

        public async Task<Result<UserResponse>> Delete(string userId)
        {
            var response = await ValidateUser(userId);
            if (!response.Success)
            {
                return Result<UserResponse>.Fail(response.Message!, response.ErrorType!);
            }
            var result = await _userManager.DeleteAsync(response.Data!);
            return ResponseResult.FromIdentityResult(result, "User created successfully", response.Data!.ToUserResponse());

        }

        public async Task<Result<string>> ForgottenPasswordAsync(string email)
        {
            if (email == null)
            {
                return Result<string>.Fail("Invalid email address.", ErrorType.Validation);
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<string>.Ok(string.Empty);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Result<string>.Ok(token);
        }

        public async Task<Result<List<UserResponse>>> GetAllUsers()
        {
            var users = await _userManager.Users.Select(x => x.ToUserResponse()).ToListAsync();

            return Result<List<UserResponse>>.Ok(users);

        }

        public async Task<Result<UserResponse>> GetUserById(string userId)
        {
            var response = await ValidateUser(userId);
            if (!response.Success)
            {
                return Result<UserResponse>.Fail(response.Message!, response.ErrorType!);
            }
            return Result<UserResponse>.Ok(response.Data!.ToUserResponse());
        }
        public async Task<Result<UserResponse>> GetUserByEmail(string userId)
        {
            var response = await ValidateUserEmail(userId);
            if (!response.Success)
            {
                return Result<UserResponse>.Fail(response.Message!, response.ErrorType!);
            }
            return Result<UserResponse>.Ok(response.Data!.ToUserResponse());
        }

        public async Task<Result<UserResponse>> LoginAsync(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result<UserResponse>.Fail("please enter email address.", ErrorType.Validation);

            }
            if (string.IsNullOrEmpty(password))
            {
                return Result<UserResponse>.Fail("please enter password.", ErrorType.Validation);

            }
            email = email.Trim().ToLowerInvariant();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.UserName is null)
            {
                return Result<UserResponse>.Fail("Invalid email or password.", ErrorType.Validation);
            }
            var signin = await _signinManager.PasswordSignInAsync(user.UserName, password, rememberMe, true);
            if (signin.Succeeded)
            {
                return Result<UserResponse>.Ok(user.ToUserResponse());

            }
            if (signin.IsLockedOut)
            {
                return Result<UserResponse>.Fail("Account locked try again later.", ErrorType.Validation);

            }
            if (signin.IsNotAllowed)
            {
                return Result<UserResponse>.Fail("Please confirm your email before logging in", ErrorType.Validation);

            }
            return Result<UserResponse>.Fail("Invalid email or password.", ErrorType.Validation);

        }

        public async Task<Result<bool>> LogoutAsync()
        {
            await _signinManager.SignOutAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<UserResponse>> ResetPasswordAsync(string email, string newPassword, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(token))
            {
                return Result<UserResponse>.Fail("Invalid password reset request.", ErrorType.Validation);

            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<UserResponse>.Fail("invalid password reset request.", ErrorType.Validation);

            }
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword.Trim());
            if (result.Succeeded)
            {
                await LogoutAsync();
                return Result<UserResponse>.Ok(user.ToUserResponse());
            }
            string error = string.Join(",", result.Errors.Select(e => e.Description));

            return Result<UserResponse>.Fail(error, ErrorType.Validation);
        }

        public async Task<Result<List<UserResponse>>> Search(string search, int page = 1, int pageSize = 30)
        {

            var query = _unitOfWork.Users.Query().Where(q => q.Email!.Contains(search) ||
             q.FirstName!.Contains(search) || q.LastName!.Contains(search));

            var users = await query.OrderBy(u => u.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).Select(u => u.ToUserResponse()).ToListAsync();
            return Result<List<UserResponse>>.Ok(users);
        }

        public async Task<Result<UserResponse>> Update(UpdateUser user)
        {
            var response = await ValidateUser(user.UserId);

            if (!response.Success)
            {
                return Result<UserResponse>.Fail(response.Message, response.ErrorType!);
            }
            var userResponse = response.Data;


            userResponse.FirstName = user.FirstName;
            userResponse.LastName = user.LastName;
            userResponse.Email = user.Email;
            userResponse.PhoneNumber = user.PhoneNumber;

            await _userManager.UpdateAsync(userResponse);
            return Result<UserResponse>.Ok(userResponse.ToUserResponse());

        }
        private async Task<Result<ApplicationUser>> ValidateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<ApplicationUser>.Fail("Invalid user id", ErrorType.Validation);
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result<ApplicationUser>.Fail("User not found", ErrorType.Not_Found);
            }
            return Result<ApplicationUser>.Ok(user);
        }
        private async Task<Result<ApplicationUser>> ValidateUserEmail(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<ApplicationUser>.Fail("Invalid user id", ErrorType.Validation);
            }

            var query = _unitOfWork.Users.Query();
            var user = await query.FirstOrDefaultAsync(u => u.NormalizedEmail == userId.ToUpper());

            if (user == null)
            {
                return Result<ApplicationUser>.Fail("User not found", ErrorType.Not_Found);
            }
            return Result<ApplicationUser>.Ok(user);
        }

    }
}
