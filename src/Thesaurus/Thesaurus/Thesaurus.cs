namespace Thesaurus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using global::Thesaurus.Models;

    public sealed class Thesaurus : IThesaurus, IDisposable
    {
        private readonly ThesaurusContext context;
        private readonly WordHelper wordHelper;

        public Thesaurus()
        {
            context = new ThesaurusContext();
            wordHelper = new WordHelper();
        }

        public async Task<int> AddWordAsync(string word, List<string> synonyms)
        {
            if (word != null && !string.IsNullOrWhiteSpace(word))
            {
                var newMeaning = new Meaning();
                var newWord = new Word
                {
                    Word1 = word,
                    Meaning = newMeaning
                };
                var newSynonyms = wordHelper.GenerateWordList(synonyms, newMeaning);

                await context.Meanings.AddAsync(newMeaning);
                var addedWord = (await context.Words.AddAsync(newWord)).Entity;
                await context.Words.AddRangeAsync(newSynonyms);
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
            var word = context.Words.FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;
                var newSynonyms = wordHelper.GenerateWordList(synonyms, meaning);

                await context.Words.AddRangeAsync(newSynonyms);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task RemoveWordAsync(int wordId)
        {
            var word = context.Words.FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                context.Words.Remove(word);
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

        public async Task<List<string>> GetSynonymsOfWordAsync(int wordId)
        {
            var word = context.Words.FirstOrDefault(word => word.Id == wordId);
            if (word != null)
            {
                var meaning = word.Meaning;
                var synonyms = await context.Words.Where(word => word.MeaningId == meaning.Id && word.Id != wordId)
                    .Select(synonym => synonym.Word1)
                    .ToListAsync();
                return synonyms;
            }
            else
            {
                throw new InvalidOperationException($"Can't find word with ID = {wordId}.");
            }
        }

        public async Task<List<string>> GetAllWordsAsync() => await context.Words.Select(word => word.Word1).ToListAsync();

        public async Task<List<List<string>>> GetAllWordsGroupedAsync()
        {
            var meanings = await context.Meanings.ToListAsync();
            var result = new List<List<string>>();
            foreach (var meaning in meanings)
            {
                var synonymsGroup = new List<string>();

                var synonyms = context.Words.Where(word => word.MeaningId == meaning.Id);
                synonymsGroup.AddRange(synonyms.Select(word => word.Word1));

                result.Add(synonymsGroup);
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
