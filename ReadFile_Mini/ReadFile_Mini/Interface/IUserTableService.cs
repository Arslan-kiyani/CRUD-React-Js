

using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;

namespace ReadFile_Mini.Interface
{
    public interface IUserTableService
    {
        Task AddUserAsync(UserTableRequest userTableRequest);
        Task<List<TripDetails>> GetAllUsersAsync();
        Task<UserTable> GetUserByIdAsync(int userId);
        Task<int> UpdateUserAsync(int id, UserTableRequest userTableRequest);
        Task DeleteUserAsync(int userId);
    }
}
