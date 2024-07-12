using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Repository;

namespace ReadFile_Mini.Services
{
    public class RaceService : IRaceservice
    {
        private readonly IraceRepository _raceRepository;
        public RaceService(IraceRepository raceRepository)
        {

            _raceRepository = raceRepository;   

        }
        public async Task<IEnumerable<Race>> GetAllRaceAsync()
        {
            return await _raceRepository.GetAllRaceAsync();
        }
    }
    
}
