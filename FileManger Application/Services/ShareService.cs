using FileManger_Application.DTOs.SharedDto;
using FileManger_Application.Exception;
using FileManger_Application.ServiceContract;

namespace FileManger_Application.Services
{
    public class ShareService : SharedContract
    {
        public Task<Result<SharedFileResponse>> AddFile(AddSharedFileRequest request)
        {
            throw new NotImplementedException();
        }

        public Result<bool> DeleteFile(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SharedFileResponse>> GetFileById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateFile(UpdateSharedFile updateFile)
        {
            throw new NotImplementedException();
        }
    }
}
