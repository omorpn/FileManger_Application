using FileManger_Application.ServiceContract;
using FileManger_Application.UnitOfWorks;

namespace FileManger_Application.Services
{
    public class UserService : UserContract
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<UserResponse> Create(AddUserRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserResponse>> GetAllProduct()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> GetProduct(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> Search()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> Update(UpdateUser User)
        {
            throw new NotImplementedException();
        }
    }
}
