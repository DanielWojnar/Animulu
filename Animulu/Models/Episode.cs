using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ShowId { get; set; }
        [Required]
        public int EpisodeIndex { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string VideoSrc { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        

    }
}
