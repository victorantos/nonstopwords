using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class HallOfFameViewModel
    {
        public int GameGroupId {get;set;}
        public int TopType { get; set; }
        public List<TopUser> TopUsers { get; set; }
    }
}