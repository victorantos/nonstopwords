using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class Game
    {
        public Game(int number)
        {
            this.Number = number;
        }
         
        public int Number { get; set; }
        public List<Word> Words{get;set;}

    }
}