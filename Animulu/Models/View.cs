using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class View
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int EpisodeId { get; set; }
        [Required]
        public int ShowId { get; set; }
        [Required]
        public DateTime ViewDate { get; set; }
    }
}
