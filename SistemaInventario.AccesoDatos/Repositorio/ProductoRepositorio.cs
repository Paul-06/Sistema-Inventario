using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _context;
        public ProductoRepositorio(ApplicationDbContext context) : base(context) // Enviar el context a la clase base (Repositorio)
        {
            _context = context;
        }

        public void Actualizar(Producto producto)
        {
            // Obtener el registro a actualizar (de producto)
            var registro = _context.Productos.FirstOrDefault(b => b.Id == producto.Id);

            // Si encontró el registro
            if (registro != null)
            {
                if (producto.ImagenUrl != null) // Si se está enviando una imagen
                {
                    registro.ImagenUrl = producto.ImagenUrl;
                }

                registro.NumeroSerie = producto.NumeroSerie;
                registro.Descripcion = producto.Descripcion;
                registro.Precio = producto.Precio;
                registro.Costo = producto.Costo;
                registro.CategoriaId = producto.CategoriaId;
                registro.MarcaId = producto.MarcaId;
                registro.PadreId = producto.PadreId;
                registro.Estado = producto.Estado;

                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem>? ObtenerTodosDropdownLista(string obj)
        {
            if (obj.Equals("Categoria"))
            {
                return _context.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString() // Value acepta valores de tipo de string
                });
            }

            if (obj.Equals("Marca"))
            {
                return _context.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString() // Value acepta valores de tipo de string
                });
            }

            if (obj.Equals("Producto"))
            {
                return _context.Productos.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString() // Value acepta valores de tipo de string
                });
            }

            return null;
        }
    }
}
