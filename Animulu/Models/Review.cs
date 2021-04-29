using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ShowId { get; set; }
        [Required]
        public int Value { get; set; }
    }
}
