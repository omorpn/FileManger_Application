using FileManger_Application.Data;
using FileManger_Application.Model;
using FileManger_Application.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileManger_Application.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public IGenericRepository<Files> Files { get; }

        public IGenericRepository<Foldder> Folder { get; }

        public IGenericRepository<SharedItem> SharedItem { get; }
        public IGenericRepository<ApplicationSetting> ApplicationSetting { get; }
        public IGenericRepository<ApplicationUser> Users { get; }
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            Users = new GenericRepository<ApplicationUser>(_context);
            Files = new GenericRepository<Files>(_context);
            Folder = new GenericRepository<Foldder>(_context);
            SharedItem = new GenericRepository<SharedItem>(_context);
            ApplicationSetting = new GenericRepository<ApplicationSetting>(_context);
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                return;
            }
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            _transaction.Commit();
            await _context.DisposeAsync();
        }

        public async Task RollBackAsync()
        {
            _transaction?.Rollback();
            await _context.DisposeAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
