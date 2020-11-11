using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Act3U3_Razas.Models.ViewModels
{
    public class RazaViewModel
    {
        public uint Id { get; set; }
        public string Nombre { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
    }
}
