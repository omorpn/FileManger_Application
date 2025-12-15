using FileManger_Application.DTOs.UserDto;
using FileManger_Application.Exception;

namespace FileManger_Application.ServiceContract
{
    public interface UserContract
    {
        /// <summary>
        /// Creates a new user based on the specified user details.
        /// </summary>
        /// <param name="user">An <see cref="AddUserRequest"/> object containing the information required to create the user. Cannot be
        /// null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> with a
        /// <see cref="UserResponse"/> if the user is created successfully; otherwise, contains error information.</returns>
        Task<Result<UserResponse>> Create(AddUserRequest user);
        /// <summary>
        /// Asynchronously retrieves a list of all users.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> object
        /// with a list of <see cref="UserResponse"/> instances representing all users. If no users are found, the list
        /// will be empty.</returns>
        Task<Result<List<UserResponse>>> GetAllUsers();
        /// <summary>
        /// Asynchronously retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="Result{UserResponse}"/> object with the user information if found; otherwise, an error result
        /// indicating the reason for failure.</returns>
        Task<Result<UserResponse>> GetUserById(string userId);
        /// <summary>
        /// Updates the details of an existing user asynchronously.
        /// </summary>
        /// <param name="User">An <see cref="UpdateUser"/> object containing the updated user information. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> with a
        /// <see cref="UserResponse"/> representing the updated user details if the update is successful; otherwise,
        /// contains error information.</returns>
        Task<Result<UserResponse>> Update(UpdateUser User);
        /// <summary>
        /// Deletes the user with the specified identifier asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result<UserResponse>
        /// indicating the outcome of the deletion. If the operation is successful, the result includes information
        /// about the deleted user.</returns>
        Task<Result<UserResponse>> Delete(string userId);
        /// <summary>
        /// Searches for users that match the specified search criteria and returns a paginated list of user responses.
        /// </summary>
        /// <param name="search">The search term used to filter users. This can be a partial or full match against user attributes such as
        /// name or email. Cannot be null.</param>
        /// <param name="page">The zero-based page index of the results to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The maximum number of user responses to include in a single page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result object with a list of
        /// UserResponse items matching the search criteria for the specified page. The list will be empty if no users
        /// match the criteria.</returns>
        Task<Result<List<UserResponse>>> Search(string search, int page, int pageSize);
        Task<Result<UserResponse>> LoginAsync(string username, string password, bool rememberMe);
        Task<Result<bool>> LogoutAsync();
        Task<Result<string>> ForgottenPasswordAsync(string email);
        Task<Result<UserResponse>> GetUserByEmail(string userId);
        Task<Result<UserResponse>> ResetPasswordAsync(string email, string newPassword, string token);


    }
}

