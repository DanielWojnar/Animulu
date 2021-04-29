using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Models
{
    public class NavPage
    {
        public int TotPages { get; set; }
        public int MinPage { get; set; }
        public int MaxPage { get; set; }
        public int CurPage { get; set; }
    }
}
