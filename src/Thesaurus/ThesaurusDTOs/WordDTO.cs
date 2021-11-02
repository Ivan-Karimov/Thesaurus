namespace ThesaurusDTOs
{
    using System.Collections.Generic;

    public class WordDTO
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public List<string> Synonyms { get; set; }
    }
}
