using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;


namespace ReadFile_Mini.Services
{
    public class TripService : ITripService
    {

        private readonly ITripRepository _tripRepository;
        private readonly SeniorDb _context;

        public TripService(ITripRepository tripRepository,SeniorDb context)
        {
            _tripRepository = tripRepository;
            _context = context;
        }

        public async Task AddTripAsync(TripRequest tripRequest)
        {
           
            var trip = new Trip
            {
                Name = tripRequest.Name,
                StartDate = tripRequest.StartDate,
                EndDate = tripRequest.EndDate,
                Amount = tripRequest.Amount,
                Members = tripRequest.Members
            };

            // Calculate single person amount
            decimal singlePersonAmount = tripRequest.Amount / tripRequest.Members;

            await _context.Trip.AddAsync(trip);
            await _context.SaveChangesAsync();

            // Retrieve the TripId generated after insertion
            int tripId = trip.TripId;

            for (int i = 0; i < tripRequest.Members; i++)
            {
                var TripTableEntry = new UserTable
                {
                    TripId = tripId, // Use the TripId as FK
                    totalAmount = singlePersonAmount,
                    Name = "Member " + (i + 1),
                    PhoneNumber = "0322-1223221" + (i + 1),
                    Address = "cch" + (i + 1),
                    Email = "example@gmail.com",

                };

                // Add the entry to the database
                await _context.UserTable.AddAsync(TripTableEntry);
                await _tripRepository.AddTripAsync(tripRequest);
            }
        }
        public async Task<string> DeleteTripAsync(int tripId)
        {
            var trip = await _context.Trip.FindAsync(tripId);
            if (trip == null)
            {
                return ("tripId not Found ");
            }
            // Check if the user ID is referenced in any other table
            var isReferenced = await _context.Trip.AnyAsync(x => x.TripId == tripId);
            //if (isReferenced)
            //{
            //    Console.WriteLine("User ID is referenced in other tables and cannot be deleted");
            //    return ("User ID is referenced in other tables and cannot be deleted");
            //}
            if (isReferenced)
            {
                 var userReferences = await _context.UserTable.Where(x => x.TripId == tripId).ToListAsync();
                 _context.UserTable.RemoveRange(userReferences);

               // var referencesInAnotherTable = await _context.UserTrip.Where(x => x.UserId == userReferences.TripId).ToListAsync();
                // _context.AnotherTable.RemoveRange(referencesInAnotherTable);
            }

            _context.Trip.Remove(trip);
            await _tripRepository.DeleteTripAsync(tripId);
            return "Trip deleted successfully";
        }

        public async Task<IEnumerable<DestinationData>> GetAllPostGresAsync()
        {
            return await _tripRepository.GetAllPostGresAsync();
        }

        public async Task<IEnumerable<TripResponse>> GetAllTripsAsync()
        {
             return await _tripRepository.GetAllTripsAsync();
        }

        public async Task<Trip> GetTripByIdAsync(int tripId)
        {
            // Implement the logic to retrieve a trip by ID
            try
            {
                return await _tripRepository.GetTripByIdAsync(tripId);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                throw;
            }
        }
        public async Task<int> UpdateTripAsync(int tripId, TripRequest tripRequest)
        {
            // Implement the logic to update a trip
            var trip = await _tripRepository.GetTripByIdAsync(tripId);

            if (trip == null)
            {
                throw new Exception("Trip not found");
            }

            trip.Name = tripRequest.Name;
            trip.StartDate = tripRequest.StartDate;
            trip.EndDate = tripRequest.EndDate;
            trip.Amount = tripRequest.Amount;
            trip.Members = tripRequest.Members;

            return await _tripRepository.UpdateTripAsync(tripId, tripRequest);
            
           
        }
    }
}
