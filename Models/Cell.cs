using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class Cell
    {
        public string letter { get; set; }
        public  string secondLetter { get; set; }
        public bool isNewLetter { get; set; }
        public int letterNumber { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool isPlaced { get; set; }
        public int isOverlap { get; set; }
    }
}