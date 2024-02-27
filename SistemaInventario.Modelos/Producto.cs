using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de serie es requerido.")]
        [MaxLength(60, ErrorMessage = "El número de serie no debe tener más de 60 caracteres.")]
        public string? NumeroSerie { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [MaxLength(100, ErrorMessage = "La descripción no debe tener más de 100 caracteres.")]
        public string? Descripcion { get; set;}

        [Required(ErrorMessage = "El precio es requerido.")]
        public double Precio { get; set;}

        [Required(ErrorMessage = "El costo es requerido.")]
        public double Costo { get; set;}

        // Imagen no será requerido
        public string? ImagenUrl { get; set;}

        [Required(ErrorMessage = "El estado es requerido.")]
        public bool Estado { get; set;}

        // Foreign Keys
        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set;}

        // Este atributo indica que la propiedad CategoriaId es una clave externa.
        // La clave externa hace referencia a la propiedad Id de la clase Categoria.
        [ForeignKey(nameof(CategoriaId))]
        public Categoria? Categoria {  get; set; } // Permite acceder a la categoría relacionada con la propiedad CategoriaId.

        [Required(ErrorMessage = "La marca es requerida.")]
        public int MarcaId { get; set;}

        [ForeignKey(nameof(MarcaId))]
        public Marca? Marca { get; set; } // De navegación

        // Recursividad
        // Tampoco será requerido
        public int? PadreId { get; set;}
        // Navegación
        public virtual Producto? Padre { get; set; }
    }
}
