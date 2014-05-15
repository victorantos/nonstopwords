using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class PlacedWord
    {
        public IEnumerable<Cell> Cells { get; set; }
    }
}