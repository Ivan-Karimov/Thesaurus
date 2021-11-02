namespace Thesaurus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ThesaurusDTOs;

    public interface IThesaurus
    {
        Task<int> AddWordAsync(string word, string meaning, List<string> synonyms);
        Task AddSynonymsForWordAsync(int wordId, List<string> synonyms);
        Task EditWordMeaningAsync(int wordId, string meaning);
        Task RemoveWordAsync(int wordId);
        Task RemoveWordWithSynonymsAsync(int wordId);
        Task RemoveSynonymsForWordAsync(int wordId, List<int> synonymsIds);
        Task<SynonymsDTO> GetSynonymsOfWordAsync(int wordId);
        Task<List<string>> GetAllWordsAsync();
        Task<List<SynonymsDTO>> GetAllWordsGroupedAsync();
    }
}
