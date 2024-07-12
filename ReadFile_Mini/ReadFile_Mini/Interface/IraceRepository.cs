using ReadFile_Mini.Models;

namespace ReadFile_Mini.Interface
{
    public interface IraceRepository
    {
        Task<IEnumerable<Race>> GetAllRaceAsync();
    }
}
