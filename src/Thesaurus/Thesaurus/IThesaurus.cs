namespace Thesaurus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IThesaurus
    {
        Task<int> AddWordAsync(string word, List<string> synonyms);
        Task AddSynonymsForWordAsync(int wordId, List<string> synonyms);
        Task RemoveWordAsync(int wordId);
        Task RemoveWordWithSynonymsAsync(int wordId);
        Task RemoveSynonymsForWordAsync(int wordId, List<int> synonymsIds);
        Task<List<string>> GetSynonymsOfWordAsync(int wordId);
        Task<List<string>> GetAllWordsAsync();
        Task<List<List<string>>> GetAllWordsGroupedAsync();
    }
}
