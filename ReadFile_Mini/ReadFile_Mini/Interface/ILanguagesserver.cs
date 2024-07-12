using ReadFile_Mini.Models;
using ReadFile_Mini.Response;

namespace ReadFile_Mini.Interface
{
    public interface ILanguagesserver
    {
        Task<IEnumerable<Language>> GetAllLanguageAsync();
    }
}
