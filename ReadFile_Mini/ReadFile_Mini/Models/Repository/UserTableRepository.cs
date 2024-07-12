using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;


namespace ReadFile_Mini.Repository
{
    public class UserTableRepository : IUserTableRepository
    {

        private readonly SeniorDb _context;

        public UserTableRepository(SeniorDb context)
        {
            _context = context;
        }
        public async Task AddUserAsync(UserTable userTable)
        {
            _context.UserTable.Add(userTable);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TripDetails>> GetAllUsersAsync()
        {
            var tripGroupUserTable = await (from t in _context.Trip
                                            select new TripDetails
                                            {
                                                TripId = t.TripId,
                                                Name = t.Name,
                                                Amount = t.Amount,
                                                StartDate = t.StartDate,
                                                EndDate = t.EndDate,
                                                MembersCount = t.Members,
                                                Users = _context.UserTable.Where(u => u.TripId == t.TripId)
                                                            .Select(u => new UserTableResponse
                                                            {
                                                                UserId = u.UserId,
                                                                Name = u.Name,
                                                                Email = u.Email,
                                                                Address = u.Address,
                                                                PhoneNumber = u.PhoneNumber,
                                                                PaidAmount = u.PaidAmount,
                                                                totalAmount = u.totalAmount,
                                                                Balance = u.totalAmount - u.PaidAmount,
                                                                // Set other properties as needed
                                                            }).ToList(),
                                            }).ToListAsync();
            return tripGroupUserTable;
        }


        public async Task<UserTable> GetUserByIdAsync(int userId)
        {
                // Retrieve the user from the database asynchronously based on userId
                var userWithTrip = await (from user in _context.UserTable
                                          join trip in _context.Trip on user.TripId equals trip.TripId into userTrips
                                          from trip in userTrips.DefaultIfEmpty() // Perform a left join
                                          where user.UserId == userId
                                          select new UserTable
                                          {
                                              UserId = user.UserId,
                                              Name = user.Name,
                                              Email = user.Email,
                                              Address = user.Address,
                                              PhoneNumber = user.PhoneNumber,
                                              TripId = user.TripId,
                                          }).FirstOrDefaultAsync();

                return userWithTrip;
        }

        public Task<int> UpdateUserAsync(int id, UserTableRequest userTableRequest)
        {
            throw new NotImplementedException();
        }
      
    }
}
