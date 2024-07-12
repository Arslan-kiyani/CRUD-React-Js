using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;

namespace ReadFile_Mini.Interface
{
    public interface IpersonalInfoRepository
    {
        //Task<bool> AddPersonalAsync(PersonalInfo personalInfo, List<string> languages);
        Task<IEnumerable<PersonalInfo>> GetAllPersonalInfoAsync();
        Task<PersonalInfo> GetPersonalInfoByIdAsync(int id);
        Task<bool> DeletePersonalAsync(int id);
        
        Task<bool> UpdatePersonalAsync(PersonalInfo personalInfo, List<string> languages);
        Task<PersonalInfo> GetByEmailAsync(string email);
        Task<Language> GetLanguageByNameAsync(string name);
        Task AddPersonalInfoAsync(PersonalInfo personalInfo);
        Task AddPersonLanguageAsync(PersonLanguage personLanguage);
         Task SaveChangesAsync();
    }
}
