﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Act3U3_Razas.Models.ViewModels
{
    public class RazaPorPaisViewModel
    {
        public Razas Raza { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        public IFormFile Archivo { get; set; }
        public string Imagen { get; set; }
    }
}
