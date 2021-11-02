using System;
using System.Collections.Generic;

#nullable disable

namespace Thesaurus.Models
{
    public partial class Meaning
    {
        public Meaning()
        {
            Words = new HashSet<Word>();
        }

        public int Id { get; set; }
        public string Meaning1 { get; set; }

        public virtual ICollection<Word> Words { get; set; }
    }
}
