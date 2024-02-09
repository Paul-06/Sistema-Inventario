using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Collections.Generic;

namespace SistemaInventario.Areas.Admin.Controllers
{
    // Siempre debemos indicar a qué área pertenece
    // un controlador(para no tener problemas para acceder a la vista).
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo unidadTrabajo) // Servicio ya inyectado (revisar Program.cs)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var categoria = new Categoria();

            if (id is null)
            {
                // Crear una nueva categoria
                categoria.Estado = true;
                return View(categoria);
            }

            // Actualizar categoria
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault()); // GetValueOrDefault() nos permite trabajar con un valor nulo (ver el parámetro)

            if (categoria is null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Evitar solicitudes desde otras páginas
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid) // Si el modelo es válido
            {
                if (categoria.Id == 0) // Significa que es un nuevo registro
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Categoría agregada exitosamente."; // Para usarlo en _Notificaciones.cshtml
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Categoría actualizada exitosamente."; // Para usarlo en _Notificaciones.cshtml
                }

                await _unidadTrabajo.Guardar();

                return RedirectToAction(nameof(Index)); // Redirigir al Index
            }

            // Si no se hace nada (modelo inválido)
            TempData[DS.Error] = "Error al guardar los cambios."; // Para usarlo en _Notificaciones.cshtml
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Buscamos la categoria a eliminar
            var registro = await _unidadTrabajo.Categoria.Obtener(id);

            if (registro is null)
            {
                return Json(new { success = false, message = "Error al borrar la categoría." });
            }

            // En caso encuentre el registro
            _unidadTrabajo.Categoria.Remover(registro);

            // Guardar los cambios
            await _unidadTrabajo.Guardar();

            // Enviamos el mensaje de éxito
            return Json(new { success = true, message = "Categoría eliminada exitosamente." });
        }

        [ActionName("ValidarNombre")] // Lo llamaremos desde el JS
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            // Inicializamos una variable booleana en falso
            bool valor = false;

            // Obtenemos todos los elementos de la categoria
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();

            // Si el id es 0, verificamos si el nombre ya existe en la lista
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre!.ToLower().Trim() == nombre.ToLower().Trim());
            }
            // Si el id no es 0, verificamos si el nombre ya existe en la lista y que el id sea diferente
            else
            {
                valor = lista.Any(b => b.Nombre!.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }

            // Si el valor es verdadero, retornamos un objeto JSON con data igual a verdadero
            if (valor)
            {
                return Json(new { data = true });
            }

            // Si el valor es falso, retornamos un objeto JSON con data igual a falso
            return Json(new { data = false });
        }

        #endregion
    }
}
