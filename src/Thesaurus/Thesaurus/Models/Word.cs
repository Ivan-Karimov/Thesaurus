using System;
using System.Collections.Generic;

#nullable disable

namespace Thesaurus.Models
{
    public partial class Word
    {
        public int Id { get; set; }
        public string Word1 { get; set; }
        public int MeaningId { get; set; }

        public virtual Meaning Meaning { get; set; }
    }
}
