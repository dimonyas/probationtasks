using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class Message
    {
        public string Time { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Text { get; set; }
    }
}