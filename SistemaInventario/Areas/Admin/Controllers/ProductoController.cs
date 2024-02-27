using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Collections.Generic;

namespace SistemaInventario.Areas.Admin.Controllers
{
    // Siempre debemos indicar a qué área pertenece
    // un controlador(para no tener problemas para acceder a la vista).
    [Area("Admin")]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment; // Nos permitirá acceder a wwwroot y a las imágenes

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment) // Servicio ya inyectado (revisar Program.cs)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVm = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Categoria"),
                MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Marca"),
                PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Producto")
            };

            if (id is null)
            {
                // Se trata de un nuevo producto
                productoVm.Producto.Estado = true; // Establecemos el valor por defecto del estado
                return View(productoVm);
            }
            else
            {
                // Se trata de una actualización
                productoVm.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVm.Producto is null)
                {
                    return NotFound();
                }

                return View(productoVm);
            }
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Categoria,Marca"); // Importante no colocar espacios en el string
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Buscamos el producto a eliminar
            var registro = await _unidadTrabajo.Producto.Obtener(id);

            if (registro is null)
            {
                return Json(new { success = false, message = "Error al borrar el producto." });
            }

            // Remover la imagen física (la que se encuentra en el directorio)
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, registro.ImagenUrl!);

            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile); // Borramos la imagen del directorio (wwwroot/imagenes/producto)
            }

            // En caso encuentre el registro
            _unidadTrabajo.Producto.Remover(registro);

            // Guardar los cambios
            await _unidadTrabajo.Guardar();

            // Enviamos el mensaje de éxito
            return Json(new { success = true, message = "Producto eliminado exitosamente." });
        }

        [ActionName("ValidarSerie")] // Lo llamaremos desde el JS
        public async Task<IActionResult> ValidarSerie(string serie, int id = 0)
        {
            // Inicializamos una variable booleana en falso
            bool valor = false;

            // Obtenemos todos los elementos de productos
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();

            // Si el id es 0 (nuevo registro), verificamos si el serie ya existe en la lista
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie!.ToLower().Trim() == serie.ToLower().Trim());
            }
            // Si el id no es 0, verificamos si el serie ya existe en la lista y que el id sea diferente
            else
            {
                valor = lista.Any(b => b.NumeroSerie!.ToLower().Trim() == serie.ToLower().Trim() && b.Id != id);
            }

            // Si el valor es verdadero, retornamos un objeto JSON con data igual a verdadero
            if (valor)
            {
                return Json(new { data = true });
            }

            // Si el valor es falso, retornamos un objeto JSON con data igual a falso
            return Json(new { data = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques de falsificación de solicitudes entre sitios (CSRF).
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto!.Id == 0)
                {
                    // Crear nuevo producto
                    string upload = webRootPath + DS.ImagenRuta; // Ruta
                    string fileName = Guid.NewGuid().ToString(); // Guid.NewGuid() => Crea un nuevo identificador único global (GUID)
                    string extension = Path.GetExtension(files[0].FileName); // Extensión

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream); // Almacenar la imagen en memoria (temporal) con toda su extensión
                    }

                    productoVM.Producto.ImagenUrl = fileName + extension; // Setteamos el atributo ImagenUrl con el actual valor
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto); // Agregamos el producto
                }
                else
                {
                    // Actualizar producto
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking:false);

                    if (files.Count > 0) // Si se carga una nueva imagen para el producto
                    {
                        string upload = webRootPath + DS.ImagenRuta; // Ruta
                        string fileName = Guid.NewGuid().ToString(); // Guid.NewGuid() => Crea un nuevo identificador único global (GUID)
                        string extension = Path.GetExtension(files[0].FileName); // Extensión

                        // Primero, borramos la imagen anterior del producto
                        var anteriorFile = Path.Combine(upload, objProducto.ImagenUrl!);

                        if (System.IO.File.Exists(anteriorFile)) // Si existe la imagen a borrar
                        {
                            System.IO.File.Delete(anteriorFile); // Borramos la imagen del directorio 
                        }

                        // Agregamos la nueva imagen
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream); // Almacenar la imagen en memoria (temporal) con toda su extensión
                        }

                        productoVM.Producto.ImagenUrl = fileName + extension; // Setteamos el atributo ImagenUrl con el actual valor
                    }
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl; // No se cambia la imagen
                    }

                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto); // Actualizamos el producto
                }

                TempData[DS.Exitosa] = "Transacción exitosa";
                await _unidadTrabajo.Guardar();

                return View(nameof(Index));
            }

            // Si el modelo no es válido
            productoVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Categoria");
            productoVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Marca");
            productoVM.PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Producto");

            return View(productoVM);
        }

        #endregion
    }
}
