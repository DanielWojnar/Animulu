using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class TagConnection
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ShowId { get; set; }
        [Required]
        public int TagId { get; set; }
    }
}
