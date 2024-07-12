using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;


namespace ReadFile_Mini.Repository
{
    public class UserTripRepository : IUserTripRepository
    {
        private readonly SeniorDb _context;

        public UserTripRepository(SeniorDb context)
        {
            _context = context;
        }

        public async Task AddUserTripAsync(UserTripRequest userTripRequest)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserTripAsync(int userTripId)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserTripResponse>> GetAllUserTripsAsync()
        {
            var userTripResponses = await (from ut in _context.UserTrip
                                           join u in _context.UserTable on ut.UserId equals u.UserId
                                           join t in _context.Trip on u.TripId equals t.TripId
                                           select new UserTripResponse
                                           {
                                               UserTrip_Id = ut.UserTrip_Id,
                                               UserId = ut.UserId,
                                               PaymentDate = ut.PaymentDate,
                                               Amount = ut.Amount,
                                               // Assuming UserTable properties
                                               Name = u.Name,
                                               Email = u.Email,
                                               Address = u.Address,
                                               PhoneNumber = u.PhoneNumber,
                                               paidAmount = u.PaidAmount,
                                               totalAmount = u.totalAmount,
                                           })
                           .ToListAsync();

            return userTripResponses;
        }

        public async Task<UserTripResponse> GetUserTripByIdAsync(int userTripId)
        {
            var userTrip = await (from ut in _context.UserTrip
                                  join u in _context.UserTable on ut.UserId equals u.UserId
                                  join t in _context.Trip on u.TripId equals t.TripId
                                  where ut.UserTrip_Id == userTripId // Filter by UserTrip_Id
                                  select new UserTripResponse // Project to UserTripResponse
                                  {
                                      UserTrip_Id = ut.UserTrip_Id,
                                      PaymentDate = ut.PaymentDate,
                                      Amount = ut.Amount,
                                      // Assuming UserTable properties
                                      Name = u.Name,
                                      Email = u.Email,
                                      Address = u.Address,
                                      PhoneNumber = u.PhoneNumber,
                                      paidAmount = u.PaidAmount,
                                      totalAmount = u.totalAmount,
                                  })
                                .FirstOrDefaultAsync(); // Get single result

            return userTrip;
        }

        public Task<int> UpdateUserTripAsync(int id, UserTripRequest userTripRequest)
        {
            throw new NotImplementedException();
        }
    }
    
}
