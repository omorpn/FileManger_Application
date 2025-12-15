using FileManger_Application.DTOs.FilesDto;
using FileManger_Application.Exception;
using FileManger_Application.Helpers;

namespace FileManger_Application.ServiceContract
{
    public interface FileContract
    {
        /// <summary>
        /// Attempts to add a new file using the specified request parameters.
        /// </summary>
        /// <param name="request">An object containing the details of the file to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// file was added successfully.</returns>
        Task<Result<bool>> AddFile(AddFileRequest request, FileType type);
        /// <summary>
        /// Asynchronously retrieves a file by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the file to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/> with a
        /// <see cref="FileResponse"/> if the file is found; otherwise, an error result indicating the reason for
        /// failure.</returns>
        Task<Result<FileResponse>> GetFileById(Guid id, string Owner);
        /// <summary>
        /// Updates an existing file with the specified changes asynchronously.
        /// </summary>
        /// <param name="updateFile">An object containing the updated file data and metadata to apply. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// update was successful. The result is <see langword="true"/> if the file was updated; otherwise, <see
        /// langword="false"/>.</returns>
        Task<Result<bool>> UpdateFile(UpdateFile updateFile);
        /// <summary>
        /// Deletes the file identified by the specified unique identifier.
        /// </summary>
        /// <param name="fileId">The unique identifier of the file to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation. The result contains <see langword="true"/> if the
        /// file was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<Result<bool>> DeletePublicFile(string fileId);
        /// <summary>
        /// Asynchronously searches for files that match the specified search criteria and returns a paged list of
        /// results.
        /// </summary>
        /// <param name="search">The search query used to filter files. This can include keywords or phrases to match against file metadata
        /// or content.</param>
        /// <param name="page">The zero-based index of the results page to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The maximum number of results to include in a single page. Must be greater than 0.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result object with a list of
        /// FileResponse items for the specified page. The list will be empty if no files match the search criteria.</returns>
        Task<Result<List<FileResponse>>> SearchAsync(string search, int page, int pageSize);
        Task<Result<List<FileResponse>>> GetAllFilesAsync(int page, int pageSize);
        Task<Result<List<FileResponse>>> GetAllFilesAsync();
        Task<Result<List<FileResponse>>> GetFileByUserIdAsync(Guid userId);
    }
}
