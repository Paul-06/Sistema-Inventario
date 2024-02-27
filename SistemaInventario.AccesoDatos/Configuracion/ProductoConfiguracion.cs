using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System.Configuration;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    // Esta clase se utilizará para configurar Producto (modelo que será una entidad en la BD)
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto> // Será buscada por Assembly (Fluent API)
    {
        // Este método se llama automáticamente por Entity Framework Core
        // para aplicar la configuración a la entidad Producto.
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            // Establecemos las propiedades de la entidad (no navegaciones)
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NumeroSerie).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired(); // Boolean
            builder.Property(x => x.Precio).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.MarcaId).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.ImagenUrl).IsRequired(false); // No es requerido
            builder.Property(x => x.PadreId).IsRequired(false);

            // Relaciones (Aquí van las navegaciones (clases))
            // Relación 1:N
            builder.HasOne(x => x.Categoria).WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación 1:N
            builder.HasOne(x => x.Marca).WithMany()
                .HasForeignKey(x => x.MarcaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Recursividad
            builder.HasOne(x => x.Padre).WithMany()
                .HasForeignKey(x => x.PadreId)
                .OnDelete(DeleteBehavior.NoAction);

            // OnDelete() define el comportamiento de las entidades dependientes cuando se elimina la entidad principal

            /*
             * DeleteBehavario.NoAction:
             * Eliminar una "Categoria" no tiene ningún impacto en las entidades "Producto" asociadas. Permanecen vinculados 
             * a la "Categoria" eliminada, lo que podría crear problemas de integridad 
             * referencial en su base de datos.
            */
        }
    }
}
