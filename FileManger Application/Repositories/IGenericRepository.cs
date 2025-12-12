namespace FileManger_Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        void RemoveAsync(T entity);
        void Update(T entity);
        IQueryable<T> Query();
    }
}
