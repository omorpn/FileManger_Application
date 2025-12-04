using FileManger_Application.DTOs.FilesDto;

namespace FileManger_Application.ServiceContract
{
    public interface FileContract
    {
        Task<FileResponse> AddFile(AddFileRequest request);
        Task<FileResponse> GetFileById(Guid id);

        void UpdateFile(UpdateFile updateFile);

        void DeleteFile(Guid fileId);

    }
}
