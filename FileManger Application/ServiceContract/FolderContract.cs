using FileManger_Application.DTOs.FolderDto;
using FileManger_Application.Exception;

namespace FileManger_Application.ServiceContract
{
    public interface FolderContract
    {
        /// <summary>
        /// Attempts to add a new folder using the specified request parameters.
        /// </summary>
        /// <param name="request">An object containing the details required to create the folder. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// folder was added successfully.</returns>
        Task<Result<bool>> AddFolder(AddFolderRequest request);
        /// <summary>
        /// Retrieves the folder with the specified identifier.
        /// </summary>
        /// <param name="folderId">The unique identifier of the folder to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> with a
        /// <see cref="FolderResponse"/> if the folder is found; otherwise, an error result.</returns>
        Task<Result<FolderResponse>> GetFolderById(string folderId, string owner);
        /// <summary>
        /// Updates the properties of an existing folder based on the specified update information.
        /// </summary>
        /// <param name="updateFolder">An object containing the updated folder properties and identifiers. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// folder was successfully updated.</returns>
        Task<Result<bool>> UpdateFolder(FolderUpdate updateFolder);
        /// <summary>
        /// Deletes the folder identified by the specified folder ID.
        /// </summary>
        /// <param name="folderId">The unique identifier of the folder to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation. The result contains <see langword="true"/> if the
        /// folder was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<Result<bool>> DeleteFolder(string folderId);
        /// <summary>
        /// Retrieves all folders owned by the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose folders are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result object with a list of
        /// FolderResponse instances representing the user's folders. The list is empty if the user does not own any
        /// folders.</returns>
        Task<Result<List<FolderResponse>>> GetAllFolderByOwner(string userId);
        /// <summary>
        /// Searches for folders that match the specified search criteria and returns a paginated list of results.
        /// </summary>
        /// <param name="search">The search term used to filter folders. Can be a partial or full folder name. Cannot be null or empty.</param>
        /// <param name="page">The zero-based index of the results page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The maximum number of folder results to include in a single page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result object with a list of
        /// FolderResponse items matching the search criteria for the specified page. The list will be empty if no
        /// folders match.</returns>
        Task<Result<List<FolderResponse>>> Search(string search, int page, int pageSize);
    }
}
