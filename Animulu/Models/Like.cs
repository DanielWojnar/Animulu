using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int EpisodeId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool Positive { get; set; }
    }
}
