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
    public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public BodegaRepositorio(ApplicationDbContext context) : base(context) // Enviar el context al padre
        {
            _context = context;
        }

        public void Actualizar(Bodega bodega)
        {
            // Obtener el registro a actualizar (de bodega)
            var registro = _context.Bodegas.FirstOrDefault(b => b.Id == bodega.Id);

            // Si encontró el registro
            if (registro != null)
            {
                registro.Nombre = bodega.Nombre;
                registro.Descripcion = bodega.Descripcion;
                registro.Estado = bodega.Estado;

                _context.SaveChanges();
            }
        }
    }
}
