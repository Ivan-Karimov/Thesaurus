namespace Thesaurus
{
    using System.Collections.Generic;
    using global::Thesaurus.Models;

    internal sealed class WordHelper
    {
        internal List<Word> GenerateWordList(List<string> words, Meaning meaning)
        {
            var wordList = new List<Word>();
            foreach (var word in words)
            {
                wordList.Add(new Word { Word1 = word, Meaning = meaning });
            }
            return wordList;
        }
    }
}
