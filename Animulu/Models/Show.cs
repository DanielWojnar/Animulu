using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class Show
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string CoverImg { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}
