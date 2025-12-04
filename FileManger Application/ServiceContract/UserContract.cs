namespace FileManger_Application.ServiceContract
{
    public interface UserContract
    {
        Task<UserResponse> Create(AddUserRequest user);
        Task<List<UserResponse>> GetAllProduct();
        Task<UserResponse> GetProduct(Guid userId);
        Task<UserResponse> Update(UpdateUser User);
        Task<UserResponse> Delete(Guid userId);
        Task<UserResponse> Search();

    }
}

,