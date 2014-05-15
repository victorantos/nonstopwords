using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class GameOverModel
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }
}