//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wordament
{
    using System;
    using System.Collections.Generic;
    
    public partial class Word
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
