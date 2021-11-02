namespace ThesaurusDTOs
{
    using System.Collections.Generic;

    public class SynonymsDTO
    {
        public string Meaning { get; set; }
        public List<WordDTO> Synonyms { get; set; }
    }
}
