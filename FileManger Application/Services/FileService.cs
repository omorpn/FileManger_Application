using FileManger_Application.DTOs.FilesDto;
using FileManger_Application.Exception;
using FileManger_Application.Helpers;
using FileManger_Application.Model;
using FileManger_Application.ServiceContract;
using FileManger_Application.UnitOfWorks;

namespace FileManger_Application.Services
{
    public class FileService : FileContract
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageContract _storageContract;
        private readonly ILogger<FileService> _logger;
        public FileService(IUnitOfWork unitOfWork, IStorageContract storageContract, ILogger<FileService> logger)
        {
            _unitOfWork = unitOfWork;
            _storageContract = storageContract;
            _logger = logger;
        }
        public async Task<Result<bool>> AddFile(AddFileRequest request, FileType type)
        {

            if (request == null)
            {
                return Result<bool>.Fail(" Invalid file details.", ErrorType.Validation);
            }
            if (request?.File == null || request.File.Count == 0)
            {
                return Result<bool>.Fail(" Invalid file details.", ErrorType.Validation);
            }
            if (string.IsNullOrWhiteSpace(request.FolderId.ToString()) && await _unitOfWork.Folder.GetByIdAsync(request.FolderId) == null)
            {
                return Result<bool>.Fail(" Invalid folder ID.", ErrorType.Validation);
            }
            if (await _unitOfWork.Users.GetByIdAsync(request.OwnerId) == null)
            {
                return Result<bool>.Fail(" Invalid User ID.", ErrorType.Validation);
            }
            await _unitOfWork.BeginTransactionAsync();
            var filePath = new List<string>();
            try
            {

                foreach (var file in request.File)
                {
                    Files newFile = new()
                    {
                        FolderId = request.FolderId,
                        FileName = file.FileName,
                        OwnerId = request.OwnerId,
                        Type = file.ContentType,
                        Size = file.Length,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    await _unitOfWork.Files.AddAsync(newFile);
                    Result<string> savedFile = type switch
                    {
                        FileType.Private => await _storageContract.SavePrivateFileAsync(file),
                        FileType.Public => await _storageContract.SavePublicFileAsync(file),
                        _ => Result<string>.Fail("Invalid file type", ErrorType.Server)
                    };

                    if (!savedFile.Success || savedFile.Data == null)
                    {
                        return Result<bool>.Fail(savedFile.Message, ErrorType.Server);
                    }
                    filePath.Add(savedFile.Data);

                    newFile.Path = savedFile.Data;
                }
                var result = await _unitOfWork.CommitAsync();
                _logger.LogInformation(result.ToString());

                return Result<bool>.Ok(true);
            }
            catch
            {
                foreach (var file in filePath)
                {
                    File.Delete(file);
                }
                await _unitOfWork.RollBackAsync();
                return Result<bool>.Fail("Somthing went while adding file", ErrorType.Server);

            }


        }


        public async Task<Result<FileResponse>> GetFileById(Guid id, string Owner)
        {
            if (!Guid.TryParse(Owner, out Guid userId))
            {
                return Result<FileResponse>.Fail("Invalid user ID.", ErrorType.Validation);
            }
            var result = await _unitOfWork.Files.GetByIdAsync(id);
            if (result.OwnerId != userId)
            {
                return Result<FileResponse>.Fail("Access Deny .", ErrorType.Unauthorised);
            }

            if (result == null)
            {
                return Result<FileResponse>.Fail("File not found.", ErrorType.Validation);
            }
            return Result<FileResponse>.Ok(result.ToFileResponse());
        }

        public async Task<Result<List<FileResponse>>> SearchAsync(string search, int page, int pageSize)
        {
            var query = _unitOfWork.Files.Query();
            DateTime.TryParse(search, out DateTime s);
            var searchResult = query.Where(p => p.FileName == search || p.CreatedAt == s);
            var response = searchResult.Order().Skip((page - 1) * pageSize).Take(pageSize).Select(file => file.ToFileResponse());
            return Result<List<FileResponse>>.Ok(response.ToList());
        }


        public async Task<Result<bool>> DeletePublicFile(string fileId)
        {
            //Try Prase the file id 
            if (!Guid.TryParse(fileId, out Guid filePath))
            {
                return Result<bool>.Fail("Invalid file ID", ErrorType.Validation);
            }
            var file = await _unitOfWork.Files.GetByIdAsync(filePath);
            if (file.Path == null)
            {
                return Result<bool>.Fail("Invalid file ", ErrorType.Validation);
            }
            var markedFilePath = Path.Combine(await _storageContract.GetPublicFile(), file.Path.Replace("/", "\\"));
            if (File.Exists(markedFilePath))
            {
                var result = await _storageContract.DeleteFileByPath(markedFilePath);
                return Result<bool>.Ok(result.Data);
            }
            return Result<bool>.Fail("Falid to delete file something went wrong", ErrorType.Server);
        }

        public async Task<Result<bool>> UpdateFile(UpdateFile updateFile)
        {
            if (updateFile == null)
            {
                Result<FileResponse>.Fail("Invalid file details.", ErrorType.Validation);
            }

            if (!Guid.TryParse(updateFile?.Id, out Guid fileId))
            {
                return Result<bool>.Fail("Invalid user id", ErrorType.Validation);
            }
            var retrivedFile = await _unitOfWork.Files.GetByIdAsync(fileId);
            if (retrivedFile == null)
            {
                return Result<bool>.Fail("File does not exist.", ErrorType.Not_Found);

            }
            if (retrivedFile.OwnerId != updateFile.OwnerId)
            {
                return Result<bool>.Fail("Access Deny .", ErrorType.Unauthorised);

            }
            if (string.IsNullOrWhiteSpace(updateFile.FileName))
            {
                retrivedFile.FileName = updateFile.FileName;
            }
            if (_unitOfWork.Folder.GetByIdAsync(updateFile.FolderId) != null)
            {
                retrivedFile.FileName = updateFile.FileName;
            }

            _unitOfWork.Files.Update(retrivedFile);
            var result = await _unitOfWork.CompleteAsync();
            return Result<bool>.Ok(result > 0);

        }

        public Task<Result<List<FileResponse>>> GetAllFilesAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Files.Query();
            var response = query.Order().Skip((page - 1) * pageSize).Take(pageSize).Select(file => file.ToFileResponse());
            return Task.FromResult(Result<List<FileResponse>>.Ok(response.ToList()));
        }

        public async Task<Result<List<FileResponse>>> GetAllFilesAsync()
        {
            var files = await _unitOfWork.Files.GetAllAsync();
            return Result<List<FileResponse>>.Ok(files.Select(file => file.ToFileResponse()).ToList());
        }

        public async Task<Result<List<FileResponse>>> GetFileByUserIdAsync(Guid userId)
        {
            var files = await _unitOfWork.Files.GetAllAsync();
            return Result<List<FileResponse>>.Ok(files.Where(file => file.OwnerId == userId).OrderByDescending(u => u.CreatedAt).Select(file => file.ToFileResponse()).ToList());
        }
    }
}


