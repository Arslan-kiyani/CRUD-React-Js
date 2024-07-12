using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Repository
{
    public class RaceRepository : IraceRepository
    {
        private readonly SeniorDb _seniorDb;
        public RaceRepository(SeniorDb seniorDb)
        {
            _seniorDb = seniorDb;
        }

        public async Task<IEnumerable<Race>> GetAllRaceAsync()
        {
            return await _seniorDb.Race.ToListAsync();
        }
    }
}
