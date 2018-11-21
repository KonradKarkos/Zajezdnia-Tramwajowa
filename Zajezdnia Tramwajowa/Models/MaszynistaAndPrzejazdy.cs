using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zajezdnia_Tramwajowa.Models
{
    public class MaszynistaAndPrzejazdy
    {
        public Maszynista M { get; set; }
        public IEnumerable<Przejazd> Przejazdy { get; set; }
    }
}