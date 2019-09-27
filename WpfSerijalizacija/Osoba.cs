using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSerijalizacija
{
    class Osoba
    {
        public int OsobaId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public override string ToString()
        {
            return $"{Ime} {Prezime}";
        }
    }
}
