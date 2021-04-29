using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class EpisodeDisplay
    {
        public int Id { get; set; }
        public int ShowId { get; set;}
        public int EpisodeIndex { get; set; }
        public string CoverImg { get; set; }
        public string ShowTitle { get; set; }
        public int Views { get; set; }
    }
}
