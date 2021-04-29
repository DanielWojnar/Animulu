using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class ShowDisplay
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImg { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Score { get; set; }
    }
}
