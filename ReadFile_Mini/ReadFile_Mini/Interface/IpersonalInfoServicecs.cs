using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;

namespace ReadFile_Mini.Interface
{
    public interface IpersonalInfoServicecs
    {
        //Task<bool> AddPersonalAsync(personalRequest personalRequest);

        Task<bool> AddPersonalAsync(personalRequest personalRequest, List<string> languages);
        Task<bool> DeletePersonalAsync(int id);
        Task<bool> UpdatePersonalAsync(int id, personalRequest updatedPersonalRequest);
        Task<IEnumerable<PersonalInfo>> GetAllPersonalInfoAsync();
        Task<PersonalInfo> GetPersonalInfoByIdAsync(int id);
    }
}
