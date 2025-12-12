

using FileManger_Application.DTOs.SharedDto;
using FileManger_Application.Exception;

namespace FileManger_Application.ServiceContract
{
    public interface SharedContract
    {
        Task<Result<SharedFileResponse>> AddFile(AddSharedFileRequest request);
        Task<Result<SharedFileResponse>> GetFileById(Guid id);

        Result<bool> UpdateFile(UpdateSharedFile updateFile);

        Result<bool> DeleteFile(Guid fileId);
    }
}
