using ReadFile_Mini.Models;

namespace ReadFile_Mini.Interface
{
    public interface IRaceservice
    {
        Task<IEnumerable<Race>> GetAllRaceAsync();
    }
}
