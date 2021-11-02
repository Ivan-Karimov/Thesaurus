namespace Thesaurus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using global::Thesaurus.Models;
    using ThesaurusDTOs;

    public sealed class Thesaurus : IThesaurus, IDisposable
    {
        private readonly ThesaurusContext context;
        private readonly WordHelper wordHelper;

        public Thesaurus()
        {
            context = new ThesaurusContext();
            wordHelper = new WordHelper();
        }

        public async Task<int> AddWordAsync(string word, string meaning, List<string> synonyms)
        {
            if (word != null && !string.IsNullOrWhiteSpace(word))
            {
                var newMeaning = new Meaning
                {
                    Meaning1 = string.IsNullOrWhiteSpace(meaning) ? null : meaning,
                };

                var newWord = new Word
                {
                    Word1 = word,
                    Meaning = newMeaning
                };

                await context.Meanings.AddAsync(newMeaning);

                if (synonyms != null)
                {
                    var newSynonyms = wordHelper.GenerateWordList(synonyms.Distinct().Except(new[] { word }), newMeaning);
                    await context.Words.AddRangeAsync(newSynonyms);
                }

                var addedWord = (await context.Words.AddAsync(newWord)).Entity;
                await context.SaveChangesAsync();

                return await Task.FromResult(addedWord.Id);
            }
            else
            {
                throw new ArgumentNullException(nameof(word));
            }
        }

        public async Task AddSynonymsForWordAsync(int wordId, List<string> synonyms)
        {
            var word = context.Words.Include(w => w.Meaning).FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;
                var existSynonyms = await context.Words.Where(w => w.MeaningId == meaning.Id).Select(s => s.Word1).ToListAsync();
                var newSynonyms = wordHelper.GenerateWordList(synonyms.Distinct().Except(existSynonyms), meaning);

                await context.Words.AddRangeAsync(newSynonyms);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task EditWordMeaningAsync(int wordId, string meaning)
        {
            var word = context.Words.FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                word.Meaning.Meaning1 = meaning;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task RemoveWordAsync(int wordId)
        {
            var word = context.Words.Include(w => w.Meaning).FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;
                var synonymsCount = context.Words.Where(w => w.MeaningId == meaning.Id).Count();
                context.Words.Remove(word);
                if (synonymsCount == 1)
                {
                    context.Meanings.Remove(meaning);
                }
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task RemoveWordWithSynonymsAsync(int wordId)
        {
            var word = context.Words.FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;
                var synonyms = context.Words.Where(word => word.MeaningId == meaning.Id);
                context.Words.Remove(word);
                context.Words.RemoveRange(synonyms);
                context.Meanings.Remove(meaning);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task RemoveSynonymsForWordAsync(int wordId, List<int> synonymsIds)
        {
            var word = context.Words.FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;

                foreach (var synonymId in synonymsIds)
                {
                    if (synonymId == wordId)
                    {
                        throw new InvalidOperationException("The word is not synonymous with itself.");
                    }

                    var synonym = context.Words.FirstOrDefault(word => word.Id == synonymId);
                    if (synonym != null)
                    {
                        if (synonym.MeaningId == meaning.Id)
                        {
                            context.Words.Remove(synonym);
                        }
                        else
                        {
                            throw new InvalidOperationException($"A synonym with ID = {synonym.Id} don't belongs to word with ID = {word.Id}.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"Can't find synonym with ID = {synonymId}.");
                    }
                }
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task<SynonymsDTO> GetSynonymsOfWordAsync(int wordId)
        {
            var word = context.Words.Include(w => w.Meaning).FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;
                var synonyms = await context.Words.Where(word => word.MeaningId == meaning.Id).ToListAsync();

                return new SynonymsDTO
                {
                    Synonyms = synonyms.Select(s => new WordDTO { Id = s.Id, Word = s.Word1 }).ToList(),
                    Meaning = meaning.Meaning1
                };
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task<List<string>> GetAllWordsAsync() => await context.Words.Select(word => word.Word1).ToListAsync();

        public async Task<List<SynonymsDTO>> GetAllWordsGroupedAsync()
        {
            var meanings = await context.Meanings.ToListAsync();
            var result = new List<SynonymsDTO>();
            foreach (var meaning in meanings)
            {
                var synonyms = await context.Words.Where(word => word.MeaningId == meaning.Id).ToListAsync();

                result.Add(new SynonymsDTO
                {
                    Synonyms = synonyms.Select(s => new WordDTO { Id = s.Id, Word = s.Word1 }).ToList(),
                    Meaning = meaning.Meaning1
                });
            }

            return result;
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
