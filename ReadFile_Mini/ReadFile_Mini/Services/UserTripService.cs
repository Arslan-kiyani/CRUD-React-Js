using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;


namespace ReadFile_Mini.Services
{
    public class UserTripService : IUserTripService
    {
        private readonly IUserTripRepository _userTripRepository;
        private readonly SeniorDb _seniorDb;
        //private readonly IUserTripService _userTripService;
        public UserTripService(IUserTripRepository userTripRepository, SeniorDb seniorDb)
        {
            _userTripRepository = userTripRepository;
            _seniorDb = seniorDb;
            //_userTripService = userTripService;
        }

        public async Task AddUserTripAsync(UserTripRequest userTripRequest)
        {
            // Map the UserTripRequest object to a UserTrip entity
            var userTrip = new UserTrip
            {
                UserId = userTripRequest.UserId,
                Amount = userTripRequest.Amount,
                PaymentDate = userTripRequest.PaymentDate
            };
            await _seniorDb.UserTrip.AddAsync(userTrip);
           

            var userTable = await _seniorDb.UserTable.FirstOrDefaultAsync(ut => ut.UserId == userTripRequest.UserId);
            if (userTable != null)
            {
                decimal newPaidAmount = userTable.PaidAmount + userTripRequest.Amount;

                if (newPaidAmount <= userTable.totalAmount)
                {
                    userTable.PaidAmount = newPaidAmount;
                    await _seniorDb.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Paid amount cannot exceed total amount.");
                }
            }
            await _userTripRepository.AddUserTripAsync(userTripRequest);
        }


        public async Task DeleteUserTripAsync(int userTripId)
        {

            var userTrip = await _seniorDb.UserTrip.FindAsync(userTripId);
            if (userTrip != null)
            {
                var relatedUsers = _seniorDb.UserTable.Where(u => u.UserId == userTrip.UserId).ToList();

                _seniorDb.UserTable.RemoveRange(relatedUsers);
                _seniorDb.UserTrip.Remove(userTrip);
            }
            await _userTripRepository.DeleteUserTripAsync(userTripId);
        }

        public async Task<IEnumerable<UserTripResponse>> GetAllUserTripsAsync()
        {
            return await _userTripRepository.GetAllUserTripsAsync();
        }

        public async Task<UserTripResponse> GetUserTripByIdAsync(int userTripId)
        {
            return await _userTripRepository.GetUserTripByIdAsync(userTripId);
        }

        public async Task<int> UpdateUserTripAsync(int id, UserTripRequest userTripRequest)
        {
            return await _userTripRepository.UpdateUserTripAsync(id, userTripRequest);
        }
    }
}
