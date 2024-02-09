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
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public MarcaRepositorio(ApplicationDbContext context) : base(context) // Enviar el context a la clase base (Repositorio)
        {
            _context = context;
        }

        public void Actualizar(Marca marca)
        {
            // Obtener el registro a actualizar (de marca)
            var registro = _context.Marcas.FirstOrDefault(b => b.Id == marca.Id);

            // Si encontró el registro
            if (registro != null)
            {
                registro.Nombre = marca.Nombre;
                registro.Descripcion = marca.Descripcion;
                registro.Estado = marca.Estado;

                _context.SaveChanges();
            }
        }
    }
}
