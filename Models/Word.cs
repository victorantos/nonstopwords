using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wordament.Models
{
    public class Word
    {
        public Word()
        {
            this.GameVariants = new HashSet<GameVariant>();
            this.GameWords = new HashSet<GameWord>();
        }

        public int WordId { get; set; }
        public string Word1 { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual ICollection<GameVariant> GameVariants { get; set; }
        public virtual ICollection<GameWord> GameWords { get; set; }
    }
}
