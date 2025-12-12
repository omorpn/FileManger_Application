using FileManger_Application.Exception;

namespace FileManger_Application.ServiceContract
{
    public interface IStorageContract
    {
        Task<Result<string>> SavePublicFileAsync(IFormFile files);
        Task<Result<string>> SavePrivateFileAsync(IFormFile files);
        Task<string> GetPublicFile();
        Task<string> GetPrivateFile();
        Task<Result<bool>> DeleteFileByPath(string path);


    }
}
