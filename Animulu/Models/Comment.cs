using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int EpisodeId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
