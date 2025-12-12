using FileManger_Application.DTOs.FolderDto;
using FileManger_Application.Exception;
using FileManger_Application.Model;
using FileManger_Application.ServiceContract;
using FileManger_Application.UnitOfWorks;

namespace FileManger_Application.Services
{
    public class FolderService : FolderContract
    {
        private readonly IUnitOfWork _unitOfWork;
        public FolderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> AddFolder(AddFolderRequest request)
        {
            if (request == null)
            {
                return Result<bool>.Fail("Invalid folder creation request", ErrorType.Validation);
            }
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.OwnerId))
            {
                return Result<bool>.Fail("Invalid folder creation request", ErrorType.Validation);
            }
            if (!Guid.TryParse(request.OwnerId, out Guid userId))
            {
                return Result<bool>.Fail("Invalid user id", ErrorType.Validation);
            }
            Foldder folder = new Foldder()
            {
                Name = request.Name,
                OwnerId = userId,
                parentFolderId = request.parentFolderId,
                CreatedAt = DateTime.Now,
            };
            await _unitOfWork.Folder.AddAsync(folder);
            var result = await _unitOfWork.CompleteAsync();
            return Result<bool>.Ok(result > 1);
        }

        public async Task<Result<bool>> DeleteFolder(string folderId)
        {
            if (Guid.TryParse(folderId, out Guid id))
            {
                return Result<bool>.Fail("Invalid folder id", ErrorType.Validation);
            }
            Foldder folder = await _unitOfWork.Folder.GetByIdAsync(id);
            if (folder == null)
            {
                return Result<bool>.Fail("folder not found", ErrorType.Not_Found);
            }
            _unitOfWork.Folder.RemoveAsync(folder);
            var result = await _unitOfWork.CompleteAsync();
            return Result<bool>.Ok(result > 1);
        }

        public async Task<Result<List<FolderResponse>>> GetAllFolderByOwner(string userId)
        {
            var query = _unitOfWork.Folder.Query();
            if (!Guid.TryParse(userId, out Guid userI))
            {
                return Result<List<FolderResponse>>.Fail("Invalid user id", ErrorType.Validation);
            }
            var folders = query.Where(f => f.OwnerId == userI).ToList();
            return Result<List<FolderResponse>>.Ok(folders.Select(f => f.ToFolderResponse()).ToList());
        }

        public async Task<Result<FolderResponse>> GetFolderById(string folderId, string owner)
        {
            if (!Guid.TryParse(folderId, out Guid id))
            {
                return Result<FolderResponse>.Fail("Invalid folder id", ErrorType.Validation);
            }
            Foldder folder = await _unitOfWork.Folder.GetByIdAsync(id);
            //Owner validation
            if (!Guid.TryParse(owner, out Guid userId))
            {
                return Result<FolderResponse>.Fail("Invalid user id", ErrorType.Validation);
            }
            if (_unitOfWork.Users.GetByIdAsync(folder.OwnerId) == null || folder.OwnerId != userId)
            {
                return Result<FolderResponse>.Fail("Access Denyed ", ErrorType.Validation);

            }
            if (folder == null)
            {
                return Result<FolderResponse>.Fail("folder not found", ErrorType.Not_Found);
            }
            var response = await _unitOfWork.Folder.GetByIdAsync(id);
            return Result<FolderResponse>.Ok(response.ToFolderResponse());
        }

        public Task<Result<List<FolderResponse>>> Search(string search, int page = 1, int pageSize = 50)
        {
            var query = _unitOfWork.Folder.Query();
            var folders = query.Where(f => f.Name.Contains(search))
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToList();
            return Task.FromResult(Result<List<FolderResponse>>.Ok(folders.Select(f => f.ToFolderResponse()).ToList()));
        }

        public async Task<Result<bool>> UpdateFolder(FolderUpdate updateFolder)
        {
            if (Guid.TryParse(updateFolder.Id, out Guid id))
            {
                return Result<bool>.Fail("Invalid folder id", ErrorType.Validation);
            }
            Foldder folder = await _unitOfWork.Folder.GetByIdAsync(id);
            if (folder == null)
            {
                return Result<bool>.Fail("folder not found", ErrorType.Not_Found);
            }
            if (!string.IsNullOrEmpty(updateFolder.Name))
            {
                folder.Name = updateFolder.Name;
            }
            _unitOfWork.Folder.Update(folder);
            var result = await _unitOfWork.CompleteAsync();
            return Result<bool>.Ok(result > 1);

        }
    }

}
