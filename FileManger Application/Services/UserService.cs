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
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
            var result = await _userManager.CreateAsync(addUser, user.Password);

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
            ApplicationUser updatedUser = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            await _userManager.UpdateAsync(updatedUser);
            return Result<UserResponse>.Ok(updatedUser.ToUserResponse());

        }
        private async Task<Result<ApplicationUser>> ValidateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<ApplicationUser>.Fail("Invalid user id", ErrorType.Validation);
            }
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ApplicationUser>.Fail("User not found", ErrorType.Not_Found);
            }
            return Result<ApplicationUser>.Ok(user);
        }

    }
}
