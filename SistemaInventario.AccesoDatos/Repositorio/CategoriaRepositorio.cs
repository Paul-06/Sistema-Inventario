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
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public CategoriaRepositorio(ApplicationDbContext context) : base(context) // Enviar el context a la clase base (Repositorio)
        {
            _context = context;
        }

        public void Actualizar(Categoria categoria)
        {
            // Obtener el registro a actualizar (de categoria)
            var registro = _context.Categorias.FirstOrDefault(b => b.Id == categoria.Id);

            // Si encontró el registro
            if (registro != null)
            {
                registro.Nombre = categoria.Nombre;
                registro.Descripcion = categoria.Descripcion;
                registro.Estado = categoria.Estado;

                _context.SaveChanges();
            }
        }
    }
}
