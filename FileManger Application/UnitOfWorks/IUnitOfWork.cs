using FileManger_Application.Model;
using FileManger_Application.Repositories;

namespace FileManger_Application.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> Users { get; }
        IGenericRepository<Files> Files { get; }
        IGenericRepository<Foldder> Folder { get; }
        IGenericRepository<SharedItem> SharedItem { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();
        Task<int> CompleteAsync();

    }
}
