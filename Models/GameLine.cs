using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class GameLine
    {
        public int num { get; set; }
        
        public List<string> cells { get; set; }
    }
}