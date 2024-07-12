

using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;

namespace ReadFile_Mini.Interface
{
    public interface IUserTripService
    {
        Task AddUserTripAsync(UserTripRequest userTripRequest);
        Task DeleteUserTripAsync(int userTripId);
        Task<IEnumerable<UserTripResponse>> GetAllUserTripsAsync();
        Task<UserTripResponse> GetUserTripByIdAsync(int userTripId);
        Task<int> UpdateUserTripAsync(int id, UserTripRequest userTripRequest);
    }
}
