using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable // IDisposable permite liberar recursos
    {
        IBodegaRepositorio Bodega {  get; }
        Task Guardar();
    }
}
