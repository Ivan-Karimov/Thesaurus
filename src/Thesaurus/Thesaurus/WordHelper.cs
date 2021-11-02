namespace Thesaurus
{
    using System.Collections.Generic;
    using global::Thesaurus.Models;

    internal sealed class WordHelper
    {
        internal List<Word> GenerateWordList(IEnumerable<string> words, Meaning meaning)
        {
            var wordList = new List<Word>();
            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    wordList.Add(new Word { Word1 = word, Meaning = meaning });
                }
            }
            return wordList;
        }
    }
}
