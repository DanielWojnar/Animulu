using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class CommentDisplay
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfilePic { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
