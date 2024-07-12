using ReadFile_Mini.Models;

namespace ReadFile_Mini.Interface
{
    public interface ILanguagerepos
    {
        Task<IEnumerable<Language>> GetAllLanguageAsync();
    }
}
