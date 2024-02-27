﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos.ViewModels
{
    // Lo usaremos para manejar información compartida de producto (marca y categoría)
    public class ProductoVM
    {
        public Producto? Producto { get; set; }
        public IEnumerable<SelectListItem>? CategoriaLista { get; set; }
        public IEnumerable<SelectListItem>? MarcaLista { get; set; }
        public IEnumerable<SelectListItem>? PadreLista { get; set; }
    }
}
