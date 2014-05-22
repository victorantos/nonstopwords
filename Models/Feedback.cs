using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wordament.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string IPAddress { get; set; }
    }
}