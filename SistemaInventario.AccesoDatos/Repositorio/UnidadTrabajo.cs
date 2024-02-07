using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        // Atributo
        private readonly ApplicationDbContext _context;
        public IBodegaRepositorio Bodega { get; private set; }

        // Constructor
        public UnidadTrabajo(ApplicationDbContext context)
        {
            _context = context;
            Bodega = new BodegaRepositorio(_context);
        }

        public void Dispose()
        {
            _context.Dispose(); // Liberar memoria
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }
    }
}
