using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class GameViewModel
    {
        public Guid GameGroupId { get; set; }
        public int GameId { get; set; }
        public string Language { get; set; }
        public bool IsDaily { get; set; }
        public int Level { get; set; }
        public bool ShowStopwatch { get; set; }
        public bool ShowLanguage { get; set; }
    }
}