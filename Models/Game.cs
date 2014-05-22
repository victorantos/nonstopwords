using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class Game
    {
        public Game()
        {
            this.GameGroups = new HashSet<GameGroup>();
            this.GameWords = new HashSet<GameWord>();
        }

        public int GameId { get; set; }
        public string Name { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual ICollection<GameGroup> GameGroups { get; set; }
        public virtual ICollection<GameWord> GameWords { get; set; }
    }
}